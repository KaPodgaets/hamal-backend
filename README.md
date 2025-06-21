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
- add endpoint for cancel from citizen form
- add counter of calls retry
- add logic for next citizen record according minimum calls retries
- add field "is_dead"
- add field "is_temporary_address"

## deploy
- change nginx.conf for frontend in docker

## front end
- 



```
dotnet ef migrations script -s ./Hamal.Web -p ./Hamal.Infrastructure -o ./migration.sql
```