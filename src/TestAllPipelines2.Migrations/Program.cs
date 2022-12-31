using System;
using System.IO;
using System.Linq;
using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TestAllPipelines2.Migrations
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Development";
            Console.WriteLine($"Environment: {env}.");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connectionStringTestAllPipelines2 = new SqlConnectionStringBuilder(
                string.IsNullOrWhiteSpace(args.FirstOrDefault())
                    ? config["ConnectionStrings:TestAllPipelines2Db_Migrations_Connection"]
                    : args.FirstOrDefault())
            {
                UserID = config["DB_USER"],
                Password = config["DB_PASSWORD"]
            }.ConnectionString;

            string scriptsPath = null;
            if (args.Length == 3)
            {
                scriptsPath = args[2];
            }

            var upgraderTestAllPipelines2 =
                DeployChanges.To
                    .SqlDatabase(connectionStringTestAllPipelines2)
                    .WithScriptsFromFileSystem(
                        !string.IsNullOrWhiteSpace(scriptsPath)
                                ? Path.Combine(scriptsPath, "TestAllPipelines2Scripts")
                            : Path.Combine(Environment.CurrentDirectory, "TestAllPipelines2Scripts"))
                    .LogToConsole()
                    .Build();
            Console.WriteLine($"Now upgrading TestAllPipelines2.");
            var resultTestAllPipelines2 = upgraderTestAllPipelines2.PerformUpgrade();

            if (!resultTestAllPipelines2.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"TestAllPipelines2 upgrade error: {resultTestAllPipelines2.Error}");
                Console.ResetColor();
                return -1;
            }

            // Uncomment the below sections if you also have an Identity Server project in the solution.
            /*
            var connectionStringTestAllPipelines2Identity = string.IsNullOrWhiteSpace(args.FirstOrDefault())
                ? config["ConnectionStrings:TestAllPipelines2IdentityDb"]
                : args.FirstOrDefault();

            var upgraderTestAllPipelines2Identity =
                DeployChanges.To
                    .SqlDatabase(connectionStringTestAllPipelines2Identity)
                    .WithScriptsFromFileSystem(
                        scriptsPath != null
                            ? Path.Combine(scriptsPath, "TestAllPipelines2IdentityScripts")
                            : Path.Combine(Environment.CurrentDirectory, "TestAllPipelines2IdentityScripts"))
                    .LogToConsole()
                    .Build();
            Console.WriteLine($"Now upgrading TestAllPipelines2 Identity.");
            if (env != "Development")
            {
                upgraderTestAllPipelines2Identity.MarkAsExecuted("0004_InitialData.sql");
                Console.WriteLine($"Skipping 0004_InitialData.sql since we are not in Development environment.");
                upgraderTestAllPipelines2Identity.MarkAsExecuted("0005_Initial_Configuration_Data.sql");
                Console.WriteLine($"Skipping 0005_Initial_Configuration_Data.sql since we are not in Development environment.");
            }
            var resultTestAllPipelines2Identity = upgraderTestAllPipelines2Identity.PerformUpgrade();

            if (!resultTestAllPipelines2Identity.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"TestAllPipelines2 Identity upgrade error: {resultTestAllPipelines2Identity.Error}");
                Console.ResetColor();
                return -1;
            }
            */

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
