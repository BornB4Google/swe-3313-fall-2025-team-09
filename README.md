# Project Plan

Welcome to the project repository for **Group 9 - Born B4 Google**. Here you will find all of the artifacts, presentations, documentation, and source code for our SWE 3313 class project.  

We are building the ultimate e-commerce platform where you can finally buy what you've always dreamed of owning: Apple Music, Microsoft Office, Disney+, or even the entire Marvel Cinematic Universe. If it exists and has brand recognition, it's probably in our inventory. 

Our stack consists of C#, ASP.NET, and SQLite, because when you're selling companies worth trillions, you need enterprise grade technology running on a database that fits on a floppy disk. 

*Disclaimer: No actual corporations were acquired in the making of this project. Yet...*


## Project Presentation

Click [here](https://www.loom.com/share/4aba970bd1cf435dbdea48bd94638f1b) for a virtual tour!

## Meet The Team: 

[Amelia Ellingson](/project-plan/resumes/Amelia_Resume.md)  
[Isaac Thoman](/project-plan/resumes/Isaac_Resume.md)  
[Andrew Tressler](/project-plan/resumes/Andrew_Resume.md)  
[Amy Ward](/project-plan/resumes/Amy_Resume.md)  
[Sara Waymen](/project-plan/resumes/Sara_Resume.md)


## Team Assignments

- We have elected to split into a front-end team, a backend team, and a project manager. 

- Click [here](project-plan/team-assignments/README.md) to see a detailed breakdown of our team assignments.

## Technology Selection

- We have selected C#, ASP.NET Core, and SQLite as our tools.    

- Click [here](project-plan/technology-selection/README.md) to see a detailed breakdown of our technology selection.

## Project Gantt Chart

- Click [here](project-plan/README.md) to see a visualzation of our Gantt chart.

# Requirements

Following extensive discussions with our client, we successfully captured all required functionalities, features, and processes needed to deliver a successful project outcome. 

We defined the requirements for an online IP marketplace to include comprehensive e-commerce capabilities. The platform enables customers to create accounts, browse inventory, and complete secure purchases. Users can register, log in, add IP assets to their cart, and finalize purchases through validated payment and legal transfer workflows.  

The system includes enhanced administration features for managing users, maintaining IP inventory, and generating sales reports. Key technical requirements ensure accurate pricing and valuation, automatic removal of sold IP assets from active listings, and provides high-fidelity UI mockup and transaction documentation for a consistent user experience.  

## Requirements and Elicitation

- For complete project requirements captured during customer conversations, please refer to the detailed documentation [here](requirements/README.md)

## Use Case Diagram

- Click [here](requirements/use-case.md) for a detailed Use-Case Diagram for our project.

## Decision Table 

- Click [here](requirements/decision-table.md) for a detailed view of our decision table for all processes in Version 1.


## Presentation

- Click [here](https://www.loom.com/share/65fc793e64074c6a9bf844f5ba1cc417?sid=6ca0645b-792f-42f6-a9ee-eceecb48b7e5) to view our presentation in Loom.

# User Interface Design

- [High Fidelity User Interface Design (Marvel)](https://marvelapp.com/prototype/ag7cae1/screen/97883566)
  
- Click [here](https://www.loom.com/share/1aed421a33954cb9a11caa0761c24dda) for a virtual tour around the User Interface!

# Basic Development Setup

Before starting, ensure you have the following installed on your machine:
- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Node.js and npm](https://nodejs.org/en/download/)

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
1. Navigate to the `backend` directory:
    ```bash
   cd backend
   ```
3. Run the ASP.NET Core application:
    ```bash
    ASPNETCORE_ENVIRONMENT=Development dotnet run --project backend/backend.csproj
    ```
Because the ASPNETCORE_ENVIRONMENT variable is set to Development, the backend will automatically proxy requests to the frontend running at localhost:4200.