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

# ToDO list
- add .env file with admin credentials
- add autoseeding admin user from .env
- add https (only for production)
- add endpoint for cancel from citizen form
- add counter of calls retry
- add logic for next citizen record according minimum calls retries

## deploy
- test that cert https on server is still alive

## front end
- add api call when cancel form



```
dotnet ef migrations script -s ./Hamal.Web -p ./Hamal.Infrastructure -o ./migration.sql
```