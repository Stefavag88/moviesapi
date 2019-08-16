# moviesapi
ASP .NET Core 2.1 WebAPI with Swagger UI

 ***Prerequisites***
 - Visual Studio 2017/2019 with .NET Core SDK (>=2.1)
 - SQL Server Management Studio >= 17 
 
 **To Run:**
  1. Download or clone the project 
  2. Open SQL Server Mgmt Studio on (localDB)/MSSQLLocalDB (or other preferred instance of localDB) and run steps 3 & 4.
  3. Run the Create_Scripts.sql to create the database and tables. 
  4. Run the PopulateTables_Scripts.sql to populate the database with tested data.
  5. If you created the project at a different instance of localDB than MSSQLLocalDB, then change the value of key "MoviesDbConnection.connectionString"
  inside MoviesAPI/appsettings.json appropriately. 
  6. Make sure that MoviesAPI is set as Startup Project. 
  7. Run in debug Mode and the Swagger UI will pop up.
  
***WebAPI Functionalities Implemented:***
- Asynchronous Controller Actions.
- Movies Service (the main service providing the crud operations to be consumed by clients).
- Admin Service (Used for inserting data to database)
- Database Interaction is implemented with Entity Framework Core, with Lazy-Loading deactivated.
- Http Request/Response Logging. 
- Swagger Integration.

***Database design specs: ***
The database consists of 


***Remarks***
For the purposes of this project no User Authentication has been implemented.
Consequently all endpoints are available to be used by everyone.
The Admin Controller would be available only to specific User Roles in a real-case scenario.  
