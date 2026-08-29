# testtask


## General Information

API is designed to work with CRUD-operations on auditoriums, check auditoriums availability and reserve auditoriums. Application also allows users generate reports on reserved auditoriums and total income. 

Access to all endpoints is restricted to users who are not authorized in system and have not provided a valid JWT. Exception is checking for available auditoriums, as this information is not critical to know. 


## Technical Information

Application was builded using **ASP.NET Core Web API**. As base architecture was chosen **Clean architecture**. Data handling is performed with using **Entity Framework Core** and database is **PostgreSQL**. 
Were implemented **Repository, Unit of Work, and Result patterns**. Authentication is based on using of **JWT**. For testing were used **xUnit and Moq**. For data mapping was used **AutoMapper**. For running application and database is used **Docker**. 

## Local setup

1. Start the database and API from Docker (Tools -- Command line -- Developer Command Promt):
```
docker complose up -d
```

2. Install dotnet ef using the following command:
```
dotnet tool install --global dotnet-ef
```

3. Create a local database from migrations with the following command:
```
dotnet ef database update --project src\Api
```

For connection to API swagger use http://localhost:8080/swagger
-->
For connection to database UI use http://localhost:5050
