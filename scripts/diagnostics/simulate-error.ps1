sqlcmd -S . -Q "ALTER DATABASE SmartLogisticsDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE"
sqlcmd -S . -Q "DROP DATABASE SmartLogisticsDb"

dotnet ef database update `
  --startup-project src/SmartLogistics.Api `
  --project src/SmartLogistics.Infrastructure
