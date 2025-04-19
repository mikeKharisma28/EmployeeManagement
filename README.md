# EmployeeManagement
1. This project is created using Visual Studio 2022, .NET Core 8 using MVC architecture, SQL Server 2022 Developer edition.
2. It also uses EntityFrameworkCore latest.
3. This project also includes migration files for deploying database and tables, will be explained on the next steps.
4. Open EmployeeManagement.sln using VS version mentioned in (1).
5. Run Redis in your local machine. In my case, I used powershell to run Redis in Docker
   docker run --name my-redis -p 6379:6379 -d redis
8. Change appsettings.json for ConnectionString and Jwt settings if necessary.
9. Then open Package Manager Console
   ![image](https://github.com/user-attachments/assets/cb1767e8-0b59-453e-a643-99ab4556b4ad)
10. Execute command Update-Database to migrate to your database
11. This project is provided with api/account/register and api/account/login, can be used to create and authorize first-time user.
12. Hence, the project is ready to be tested. 
