# Technical Design

This document defines the technical architecture and implementation approach for Offshore Holdings, a full-stack web application. It provides detailed specifications for the system's structure, data models, and technology stack to ensure a robust, maintainable solution.

## A. Implementation Language(s)

The application is implemented using the following languages:

#### Backend:
- **C#**: Server-side language for ASP.NET Core, providing strong typing, extensive library support, and built-in dependency injection. Chosen because C# syntax and object-oriented principles align closely with Java, leveraging existing team expertise.

#### Frontend:
- **TypeScript**:  Angular's primary language, offering type safety and compile-time error detection. 
- **HTML**: Markup language for UI structure and templates.  
- **CSS**: Styling language for visual design and layout.

#### Data:
- **SQLite**: SQLite is a lightweight, file-based relational database that stores all data in a single file (`offshore.db`). Unlike traditional database systems that require a separate server (like MySQL or PostgreSQL), SQLite runs directly within the application, making it ideal for development and single-user applications.
- **JSON**: Data format for API communication and JWT tokens, providing lightweight and human-readable data exchange between frontend and backend.



## B. Implementation Framework(s)

- **Angular (Front End Framwork)**: Angular allows for easy creation of components and services, simplifying the development of a fast and interactive client side rendered web app. It also has very detailed established conventions and style, making it easier to maintain consistency across the team.

Here are a few useful resources related to Angular:

[Learn Angular](https://angular.dev/tutorials/learn-angular)

[Angular Essentials Overview](https://angular.dev/essentials)

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

## F. Data Examples
Click [here](assets/example-data/example-data-README.md) for example data.
## G. Database Seed Data
Click [here](assets/seed-data/seed-data-README.md) for seed data.
## H. Authentication and Authorization Plan

### Authentication

#### Login Process:  

- User enters username and password on login page.
- Client transmits credentials to server.
- Password is hashed on the client side using SHA256.
- Username and hashed password compared against `USER` table in SQLite database.
- Match found → user/admin logged in successfully.
- No match → login denied (invalid credentials).
- Upon successful login, JWT issued to user/admin.

#### Registration Process

- Client submits username and password to server.
- Password is hashed on the client-side using SHA256.
- New entry added to `USER` table with username and hashed password.
- User Automatically logged in with new credentials.

### Authorization

### Role Management

- Role is determined by `IsAdmin` field in `USER` table.
- `IsAdmin = 0` → standard user.
- `IsAdmin = 1` → administrator.

### User Permissions

- Search Inventory.
- Register and log-in.
- Complete purchases and confirm orders.
- Add/remove from cart.
- Check out.

### Administrator Permissions

- All user permission plus:
- Add items to inventory.
- Remove items from inventory.
- Generate sales report.

### Endpoint Authorization Flow:

- Client includes JWT with each API request.
- Server validates JWT and identified associated user in `USER` table.
- **If no user identified and endpoint requires authentication**: Return `401 Unauthorized`.
- **If user identified and endpoint accessible to all users**: Execute action on behalf of user.
- **If user identified and endpoint requires administrative access**: Check `IsAdmin` field.
    - `IsAdmin = 1` → Execute admin action.  
    - `IsAdmin = 0` → return `401 Unauthorized`.  


## I. Coding Style Guide

Offshore Holdings requires that standard style guides be used for all implementation to ensure maintainabilty and longevity. The following conventions must be followed: 

#### General Principles 

- Code must be documented with clear, descriptive names
- Include comments for complex logic or methods
- Keep functions/methods focused on single responsibilities. High coupling leads to low cohesion.
- Maximum file length: no more than 500 lines.
- Miximum 75 characters per line.

#### C# Backend [Microsoft Style Guide](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

- Naming:
  - Classes, methods, properties: `PascalCase`.
  - Private fields `_camelCase` with underscore prefix.

#### TypeScript/Angular [Angular Style Guide](https://angular.dev/style-guide#keep-components-and-directives-focused-on-presentation)
  
- Naming:
  - Components: `FeatureNameComponent`.
  - Services: `FeatureNameService`.
  - Component selectors: `app-feature-name`.
- One component per file. 

#### SQLite Database [SQLite Style Guide](https://www.sqlstyle.guide/)  
  
- Table names: `UPPER_CASE` (e.g., `USER`, `INVENTORY`).
- Column names: `PascalCase` (e.g., `IsAdmin`, `ProductId`).
- Always use foreign key constraints.

### Version Control and Repository Management

#### Platform: Github

**Repository Structure**

- Main repostory: `swe-3313-fall-2025-team-09`
- Branches:
  - `main`: Production ready code (protected branch).
  - `dev`: Integration branch for features.
  - `database`: Database schema changes and migrations
  - `feature` Individual feature branches.
 
**Workflow**

- All changes require pull requests (PRs).
- PRs require at least one approval before merging.
- All tests must pass before merge.
- Delete feature branch after succesful merge.

**Branch Strategy:**

- `dev` serves as the integration branch where all work is merged for testing
- `backend` and `database` branches used for their respective development work
- `feature` branch for implementing new features
- Branches merge to `dev` for integration testing before production deployment

**Commit Conventions:**
- Use clear, descriptive commit messages
- Format: `[Type] Brief description`
  - Types: `feat`, `fix`, `refactor`, `test`
- Example: `[feat] Add JWT authentication to login endpoint`

**Code Review Requirements:**
- All PRs must be reviewed by at least one team member
- Check for adherence to coding style guide
- Verify tests are included for new features
- Confirm no merge conflicts before approval

## Technical Design Presentation

Loom video goes [here](https://www.youtube.com/watch?v=dQw4w9WgXcQ) please update
