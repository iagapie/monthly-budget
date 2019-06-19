* cd src/IA.Finance.Api/

* dotnet ef migrations add InitialIdentity -o Migrations -c AppIdentityDbContext
* dotnet ef migrations add InitialFinance -o Migrations/App -c FinanceContext

* dotnet ef database update -c AppIdentityDbContext
* dotnet ef database update -c FinanceContext
