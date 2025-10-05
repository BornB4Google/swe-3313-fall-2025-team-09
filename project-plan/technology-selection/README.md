# Technology Selection

## Language
**C#**

C# was selected as our primary backend language due to its:
- Strong typing and robust tooling support
- Excellent integration with ASP.NET Core framework
- Team familiarity - all members have prior experience with C#
- Mature ecosystem with extensive documentation and community support

**TypeScript/JavaScript**

TypeScript is used for the Angular frontend, providing type safety while compiling to JavaScript for browser execution.

## Frameworks

**ASP.NET Core**
- Modern, cross-platform web framework from Microsoft
- Excellent performance and scalability
- Strong support for RESTful API development
- [Official Documentation](https://learn.microsoft.com/en-us/aspnet/core/)

**Angular**
- Modern, component-based frontend framework
- Provides strong structure and organization for complex UIs
- Static build output can be easily served by ASP.NET Core
- Comprehensive CLI tooling for development
- [Official Documentation](https://angular.dev/)

## Data Storage

**SQLite**
- Lightweight, Robust, serverless SQL database engine
- Zero-configuration setup
- Easy first-party integration with C#
- [Official Documentation](https://www.sqlite.org/)

## Operating Environment

**Development & Deployment:**
- Windows, macOS, or Linux
- .NET 9 SDK or later
- Node.js 22.x or later (for Angular development)

**Architecture:**

The application follows a client-server architecture where the Angular frontend is built as static files and served by the ASP.NET Core backend. The backend handles all API requests, business logic, and database operations with SQLite.