# Technical Design

Placeholder

## A. Implementation Language(s)

The application is implemented using ASP.NET Core with C# on the backend and Angular on the frontend. This technology stack was selected because:  
- **C# and .NET**: Provides strong typing, extensive library support, and built-in dependency injection  
- **Entity Framework Core**: Simplifies database operations with ORM capabilities  
- **Angular**: Offers a component-based architecture with TypeScript for type safety  
- **Team Familiarity**: C# syntax and object-oriented principles align closely with Java, leveraging existing team expertise  

## B. Implementation Framework(s)
We are using Angular as our front end framework. 
Isaac - explain why

## C. Data Storage Plan
### Storage Format
- **Engine:** SQLite  
- **Database File:** `offshore.db` stored on disk so data persists between application runs.

### C# Libraries / Technologies
- **Entity Framework Core 9.0** with the SQLite provider (ORM).
- **Connection string** stored in `appsettings.json`.

### Data Flow
- Angular sends HTTP requests to ASP.NET Core API endpoints.
- Controllers/services use `AppDbContext` to:
  - Query data  
  - Insert/update/delete entities  
  - Call `SaveChanges()` to write changes to the SQLite file  
- All domain objects are represented as EF Core entity classes and mapped to tables.

## D. Entity Relationship Diagram

![Entity Relationship Diagram](assets/entity-relationship-diagram.png)

## E. Entity/Field Descriptions

![Data Dictionary](assets/DataDictionary.png)

Click [here](assets/DataDictionary.png) to access the data dictionary.

## F. Data Examples
Click [here](assets/example-data-README.md) for example data.
## G. Database Seed Data
Click [here] for seed data.
## H. Authentication and Authorization Plan


## I. Coding Style Guide

Offshore Holdings requires that standard style guides be used for all implementation to ensure maintainabilty and longevity: 

- C# style guide can be located [here](https://www.sqlstyle.guide/)  
- Angular style guide can be located [here](https://angular.dev/style-guide#keep-components-and-directives-focused-on-presentation)  
- SQLLite style guide can be located [here](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)  

## Technical Design Presentation
