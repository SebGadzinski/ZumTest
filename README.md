# Zum Test

This is a full-stack application with a frontend Angular project and a backend ASP.NET Core project.

## Requirments
    NodeJS - Version >= 18.19
    dotnet
    Angular CLI
        - CMD: npm install -g @angular/cli

## How To Run

1. Clone the repo

### Visual Studio
2. Open Folder and Open solution
3. Click Start. This will install the npm packages first and then run the C# Asp Net API followed by the angular front end application.
4. Once the application browser pops up you may start a tournament

### Terminal
2. Go to folder path containing two projects
3. CMD: cd App.Server
4. CMD: dotnet build
    - This will build the dot net C# ASP.NET API project and install the npm packages for the front end.
    - --ignore-failed-sources if you have private nuget sources not accessible via the terminal
5. CMD: dotnet dev-certs https --trust
    - Only if you do not have a local cert
7. CMD: dotnet run
