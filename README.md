# EF Core 6 Fundaments via VS Code/dotnet CLI

Creating application via dotnet Command line that has a console application and class library. This project is based on the [EFCore 6 Fundamentals](https://app.pluralsight.com/library/courses/ef-core-6-fundamentals/exercise-files) by [Julie Lerman](https://app.pluralsight.com/profile/author/julie-lerman). 

## Create the solution and projects
The following commands create the overall solution and the individual projects contained within the solution.

```
dotnet new sln
dotnet new classlib -o PublisherDomain
dotnet new console -o PublisherConsole
```

## Add both projects to the solution
In order to use a unified build on the solution, both projects, after creation must be registered with the solution. This allows the entire solution to be built at once and avoid individual builds on the projects.
```
dotnet sln add PublisherDomain/PublisherDomain.csproj
dotnet sln add PublisherConsole/PublisherConsole.csproj
```

## Add a reference to the console app
In order for the console app to utilize the class library project, a reference must be created. This will allow the console application to utilized the classes within the class library.
```
cd PublisherConsole
dotnet add reference ../PublisherDomain/PublisherDomain.csproj
```

## Add a NuGet package to a project
To add the EF Core 6 package, target the specific EF Core 6 target database. This will also target all the additional dependencies that the package relies on and install them as well. **_Reminder_: Make sure your in the a project directory it will be added to.**
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

## Building the application
To build all the projects within the solution, make sure your in the root directory.
```
dotnet build
```
This will read the solution and dependencies and build the individual projects.

## Executing the project
Executing the project, we will target the console application.
```
dotnet run --project PublisherConsole/PublisherConsole.csproj
```