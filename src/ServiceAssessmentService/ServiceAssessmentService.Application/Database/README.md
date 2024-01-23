## Getting started

You need to have the dotnet ef tool installed in order to manage the database using Entity Framework.

### Install/update the dotnet ef tool

Note: global installation allows it to be used within any solution/project.

```bash
dotnet tool install --global dotnet-ef
```

```bash
dotnet tool update --global dotnet-ef
```

### Updating the database model

Changes to the database model (entities) should be managed using migrations / reflected in the database using migrations.

## Common tasks

**CAUTION - ensure you use the correct connection string:**

- See the connection string in `DataContextFactory.cs`
- **TODO**: Update this to use the connection string from e.g. local secrets (or use the Web App configuration).

### Create a new migration to reflect the current database model (entities)

```bash
## Run this command from the solution root folder
#cd  src/ServiceAssessmentService
dotnet ef migrations add <MIGRATION-NAME> --project ServiceAssessmentService.Application
```

### Remove the last migration

```bash
## Run this command from the solution root folder
#cd  src/ServiceAssessmentService
dotnet ef migrations remove --project ServiceAssessmentService.Application
```

### Create a database for the first time, or update it to include the most recent migration

```bash
## Run this command from the solution root folder
#cd  src/ServiceAssessmentService
dotnet ef database update --project ServiceAssessmentService.Application --context DataContext
```

## File locations

- Migration files are, by default, added relative to the DbContext file:
    - ```
      ## DbContext file:
      src/ServiceAssessmentService/ServiceAssessmentService.Application/Database/DataContext.cs
        
      ## Migration files:
      src/ServiceAssessmentService/ServiceAssessmentService.Application/Database/Migrations/
      src/ServiceAssessmentService/ServiceAssessmentService.Application/Database/Migrations/<MIGRATION-NAME>.cs
      src/ServiceAssessmentService/ServiceAssessmentService.Application/Database/Migrations/<MIGRATION-NAME>.Designer.cs
        
      ## Entity files:
      src/ServiceAssessmentService/ServiceAssessmentService.Application/Database/Entities/
      src/ServiceAssessmentService/ServiceAssessmentService.Application/Database/Entities/<ENTITY-NAME>.cs
      ```
  
