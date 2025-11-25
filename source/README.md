# Basic Development Setup

Before starting, ensure you have the following installed on your machine:
- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Node.js and npm](https://nodejs.org/en/download/)

Treat /source as your main project directory.

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
   
At this point, the frontend should be running at [localhost:4200](http://localhost:4200)

## Backend Setup
1. Run the ASP.NET Core application from project root (/source):
   
Linux/macOS:

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet run --project backend.csproj
```

Windows (PowerShell):

```bash
$env:ASPNETCORE_ENVIRONMENT="Development"; dotnet run --project backend.csproj
```

    
Because the ASPNETCORE_ENVIRONMENT variable is set to Development, the backend will automatically proxy requests to the frontend running at localhost:4200.

You should now be able to preview the full app at [localhost:5243](http://localhost:5243). Edits made to the frontend should also automatically reload the page.

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
