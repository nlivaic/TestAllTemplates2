# Usage: migrations_su.ps1 <previous_migration> <next_migration_number> <next_migration_name>
# Usage: migrations_su.ps1 '0001_Initial' '0003_StudycastIntegration'
$previous_migration=$args[0]
$next_migration_name=$args[1]
$full_script_path="../TestAllPipelines2.Migrations/TestAllPipelines2Scripts/" + $next_migration_name + ".sql"
cd ./src/TestAllPipelines2.Data
dotnet ef migrations add $next_migration_name --startup-project ../TestAllPipelines2.Api/TestAllPipelines2.Api.csproj --context TestAllPipelines2DbContext
if ($previous_migration -eq '')
{
    dotnet ef migrations script --startup-project ../TestAllPipelines2.Api/TestAllPipelines2.Api.csproj --context TestAllPipelines2DbContext -o $full_script_path
}
else
{
    dotnet ef migrations script --startup-project ../TestAllPipelines2.Api/TestAllPipelines2.Api.csproj --context TestAllPipelines2DbContext $previous_migration $next_migration_name -o $full_script_path
}
cd ../..