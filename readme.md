# How to get started

> All the steps outlined here are detailed further on down in the document.

1. Run docker-desktop
2. Configure Azure Service Bus
3. Run `./configure.ps1`
4. Open solution using Visual Studio, set docker-compose as Startup project and run the solution
5. Run `./create_migration.ps1 '' 'Initial migration'`
6. Run `./migrate.ps1`
7. Go to http://localhost:44395/index.html

At this point only `.gitignore` has been committed. Now you can make some changes to the source code:

1. `git checkout -b feature/initial; git add *; git commit -m "Initial code commit."; git push -u origin feature/initial`
2. You can remove or update some of the endpoints and models if you like.
3. Open `testallpipelines2_release_pipeline.yml` and set the name of your ADO project on line 43 (just search for `TestAllPipelines2_ADO_Project` and rename it).
3. Create and approve PR
3. `git checkout master; git pull`

Now configure the pipelines on ADO:

1. Add three new pipelines: `testallpipelines2_build_pipeline`, `testallpipelines2_pr_pipeline`, `testallpipelines2_release_pipeline`.

Setup your AppServices:

1. To do

Setup your Azure SQL:

1. To do

At this point you have a local environment and Azure Service Bus fully set up, along with ADO pipelines ready deploy your code to a working AppService. Start working on your features!

# Before You Get Started

## Install a Docker host

E.g. Docker Desktop:

    choco install docker-desktop

## Configure Azure Service Bus

Create a new namespace, with two Shared access policies, one for reading (called "ReaderPolicy") and one for writing (called "WriterPolicy"). Make sure both have `Manage` permission. Now find the primary connection string and copy it somewhere. You will only need the part up until the first semicolon (`Endpoint=sb://yourApplicationName.servicebus.windows.net/`). Also make a note of your policies names and keys. You will need all of these to configure the environment variables.

## Configuration

### Set configuration

Most of the stuff is in the `.env` file. This is a git ignored file, but it has the relevant structure in it. Some of the fields are prepopulated, others have to be provided by you. Execute `./configure.ps1` from the root folder and follow the instructions. It helps if you have the following beforehand:

- Connection strings and details for the Service Bus (details above).
- Username and password for the email are provided as default values, but you can provide whatever values you want.

### Database configuration

Database connection string for both `Api`, `WorkerServices` and `Migrations` projects is in the `.env` file. This was a deliberate choice, because I wanted the templated project to have a connection string automatically generated and in line with the name of the solution. You will notice there are two connection strings: `ConnectionStrings__TestAllPipelines2DbConnection` is used by `Api` and `WorkerServices`. `Migrations` has a separate one `ConnectionStrings__TestAllPipelines2Db_Migrations_Connection` because it is accessing the dockerized database from outside.

Username and password for the database are provided as default values, but you can provide whatever values you want.
Make sure you set the database-related variables (prefixed `DB_`) before you run the solution for the first time, otherwise the database will be configured with given administrator password and a username and password for the application user. If you don't change those values before running the solution you will have to delete the `testallpipelines2.sql` container and accompanying volumes. If you change `DB_PASSWORD`, make sure the same value is set in `InitializeTestAllPipelines2Db.sql` for the login as well.

When you first run the solution, an SQL script found in `src/InitializeTestAllPipelines2Db.sql` is executed, creating the database with an admin account (password in `DB_ADMIN_PASSWORD`), login and user (`DB_USER` and `DB_PASSWORD`). User is then assigned to read, write and DDL roles.

Application is accessing the database as a `DB_USER`/`DB_PASSWORD`, with a generated connection string found in `ConnectionStrings__TestAllPipelines2DbConnection` and `ConnectionStrings__TestAllPipelines2Db_Migrations_Connection`.

# Running The Application

Make sure to set the `docker-compose` as the startup project. The application can be reached by default on `localhost:44395`. You can change this in the `docker-compose.yml`. Just go to `/swagger/index.html` to see the initial API.

At this point you have several things up and running:

- API (dockerized)
- Worker service (dockerized)
- Empty Sql Server database (dockerized)
- Azure Service bus with several topics, subscriptions and queues
- nginx reverse proxy in `docker-compose.yml`
- Smtp fake email server in `docker-compose.yml`
- Seq log management service in `docker-compose.yml`

Now it is time to create some tables in the database. From the root of your solution, first run `.\create_migration.ps1 '' '0001_Initial'` and then `./migrate.ps1`. Now you have to go to the SSMS and register your database server there. It is accessible on localhost, port 1433, with the username and password you set in your `.env` file under `DB_USER` and `DB_PASSWORD`.

# Additional Stuff

## Branching strategy

Feature branches strategy is supported out of the box. This strategy expects all development to go through branches and committing directly to `master` is not allowed. Supported branches:

* `feature/`
* `fix/`

## Pipelines

There are three Azure YAML pipelines:

* `testallpipelines2_pr_pipeline`
* `testallpipelines2_build_pipeline`
* `testallpipelines2_release_pipeline`

All pipelines build and deploy all applications (`Api` and `WorkerServices`) in the solution.

When creating ADO pipelines, name them just like the files are named (minus the `.yml` suffix).

### Additional pipeline configuration

Naming the pipelines same as the files is important because the `testallpipelines2_release_pipeline` is triggered by a successful `testallpipelines2_build_pipeline` run. If you decide to name your ADO pipelines differently, make sure you change two things in `testallpipelines2_release_pipeline.yml` - update `source` on line 8 and `definition` on line 39 to match the **build** pipeline name in ADO (if needed).

`testallpipelines2_release_pipeline.yml` - `project` on line 42 should be the name of your ADO project.

**All** pipelines work with `master` branch . If you are using `main`, remember to do a search and replace.

## Project naming

All projects have a prefix `TestAllPipelines2` and pipelines latch onto that detail. If you want to start renaming projects, you should also do a search and replace across all the files in the solution. Be careful!

## Versioning

We are using semver and GitVersion. Each commit message gets a suffix (defined in `./githooks/prepare-commit-msg` and recognized in `GitVersion.yml`). `feature/` branch gets a suffix saying GitVersion should bump minor version. `fix/` branch gets a suffix saying GitVersion should bump patch version. Bumping major version needs to be done manually by tagging a commit. We do not embed the version in the assemblies yet. GitVersion depends on `--no-ff` merges to be able to deduce version successfully. Make sure your ADO project enforces this, do not allow developers to merge PRs differently! 

## Generating cert for your local development box

The template does not use HTTPS, however it can easily be added. There is a `.conf` file in there which you need to tweak to your liking. Then you need to generate `.crt` and `.key` files for Api. These make up the self-signed certificate, and the commands to create the certificate are below, with a dummy password of `rootpw`:

1. Go to **solution root folder** and execute below lines from **WSL2**:

   sudo openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout api-local.testallpipelines2.key -out api-local.testallpipelines2.crt -config api-local.testallpipelines2.conf -passin pass:rootpw

   sudo openssl pkcs12 -export -out api-local.testallpipelines2.pfx -inkey api-local.testallpipelines2.key -in api-local.testallpipelines2.crt

2. Add the certificate to your computers CA store: go to ./nginx, right-click on `.pfx` files and install to `LocalMachine` -> `Trusted Root Certification Authorities`.

For more details consult: https://bit.ly/3eWOHH2

## Hosts file

You can tell nginx to work with the `localhost`, however this might become a problem if you have multiple services running. To sidestep the issue you can keep the nginx.conf as it is, but that will require a change to `hosts` file.

    # Development DNS
    127.0.0.1	    api-local.testallpipelines2.com
    127.0.0.1	    id-local.testallpipelines2.com

## Migrations

For migrations to work `.env` file must be properly set up with database credentials and connection string configured.

### Creating migrations

The below commands must be executed from solution root folder using Powershell. If this is the first migration in your project, execute:

    .\create_migration.ps1 '' '0001_Initial'

Every next migration must contain the name of the migration immediately preceeding it:

    .\create_migration.ps1 '0001_Initial' '0002_Second'

### Applying migrations

Command must be executed from solution root folder using Powershell. You will notice it is executing from a Docker container and Docker compose - the reason is this way there is only one `.env` which can be shared by all executeable projects in the solution (`Ąpi`, `Migrations`, `WorkerServices`).

    ./migrate.ps1
