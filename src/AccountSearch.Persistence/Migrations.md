## Migration CLI Commands
Below are the commands to work with EF migrations. They should be executed from the root directory of the repo.

Create migration:
```
dotnet ef migrations add InitialCreate --project ./src/AccountSearch.Persistence --startup-project ./src/AccountSearch.Api
```

Create/update the database from migrations:
```
dotnet ef database update --startup-project ./src/AccountSearch.Api
```