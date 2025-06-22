# hamal-backend

## Migrations tips
### Add migration
```
dotnet ef migrations add Initial -s .\Hamal.Web -p .\Hamal.Infrastructure
```
### Apply migration
```
dotnet ef database update -s .\Hamal.Web -p .\Hamal.Infrastructure
```

### Undo migrations
```
dotnet ef database update 0 -s .\Hamal.Web -p .\Hamal.Infrastructure
```

### Remove migration
```
dotnet ef migrations remove -s .\Hamal.Web -p .\Hamal.Infrastructure
```
### Create sql script file for migration
```
dotnet ef migrations script -s .\Hamal.Web -p .\Hamal.Infrastructure -o .\migration.sql
```