# Technical Design

Placeholder

## A. Implementation Language(s)
Offshore Holdings will be implemented in C#, HTML, and CSS. 
C# was chosen to allow use of Angular and it's similarity to Java.

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
<img width="413" height="459" alt="ERD" src="https://github.com/user-attachments/assets/77f5a269-d379-4dc6-8b22-beef7d85da29" />

## E. Entity/Field Descriptions
Amy - fill this
## F. Data Examples
Amy - fill this
## G. Database Seed Data
Amy - fill this
## H. Authentication and Authorization Plan


## I. Coding Style Guide

Offshore Holdings requires that standard style guides be used for all implementation to ensure maintainabilty and longevity: 

- C# style guide can be located [here](https://www.sqlstyle.guide/)  
- Angular style guide can be located [here](https://angular.dev/style-guide#keep-components-and-directives-focused-on-presentation)  
- SQLLite style guide can be located [here](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)  

## Technical Design Presentation
