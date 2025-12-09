# Offshore Holdings Development Setup

## Prerequisites (for running outside a container)

Ensure you have the following installed:
- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Node.js and npm](https://nodejs.org/en/download/)

## Live Versions
- [main branch (production)](https://offshoreholdings.shop/)
- [dev branch (integration)](https://dev.offshoreholdings.shop/)

## Demo Account Credentials

| Role  | Username  | Password   |
|-------|-----------|------------|
| Admin | `award62` | `Admin1!`  |
| User  | `john`    | `password` |

These demo accounts are created automatically on application startup if no database exists.

# Basic Development Setup
Treat `/source` as your main project directory.

## Frontend Setup
1. Install the Angular CLI:
   ```bash
   npm install -g @angular/cli
   ```
2. Navigate to the `frontend` directory:
   ```bash
   cd frontend
   ```
3. Install project dependencies:
   ```bash
   npm install
   ```
4. Start the Angular development server:
   ```bash
   ng dev
   ```

At this point, the frontend should be running at [http://localhost:4200](http://localhost:4200).

## Backend Setup
1. Run the ASP.NET Core application from project root (`/source`):

   **Linux/macOS:**
   ```bash
   ASPNETCORE_ENVIRONMENT=Development dotnet run --project backend.csproj
   ```

   **Windows (PowerShell):**
   ```powershell
   $env:ASPNETCORE_ENVIRONMENT="Development"; dotnet run --project backend.csproj
   ```

Because the `ASPNETCORE_ENVIRONMENT` variable is set to `Development`, the backend will automatically proxy requests to the frontend running at `localhost:4200`.

You should now be able to preview the full app at [http://localhost:5243](http://localhost:5243). Edits made to the frontend should also automatically reload the page.

# Running in a Container
To run the application in a Docker container, ensure you have [Docker](https://www.docker.com/get-started) installed.

1. Build the Docker image:
   ```bash
   docker build -t offshore-app .
   ```
2. Run the Docker container:
   ```bash
   docker run -p 8080:8080 offshore-app
   ```

# Running Locally in Production Mode
To run the application locally in a way that closely matches production (Angular built and served by ASP.NET Core), do the following from the `/source` directory:

1. Publish the backend in Release mode (this also builds the Angular frontend and includes it in the publish output):
   ```bash
   dotnet publish -c Release -o ./publish
   ```
2. Run the published app with the `Production` environment.

   **Linux/macOS:**
   ```bash
   cd publish
   ASPNETCORE_ENVIRONMENT=Production dotnet backend.dll
   ```

   **Windows (PowerShell):**
   ```powershell
   cd publish
   $env:ASPNETCORE_ENVIRONMENT="Production"; dotnet .\backend.dll
   ```

3. Open the app in your browser using the default URL:
    - http://localhost:5000
