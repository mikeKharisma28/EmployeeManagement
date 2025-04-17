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
11. This project is provided with api/account/register and api/account/login. 
12. There are 3 roles, Admin, Approver 1 and Approver 2. Before testing, make sure you have registered users with these roles.
13. Role Admin has access to updating vehicle data and input the transaction. After a transaction is submitted, the data will need to be approved by Approver 1 and 2.
14. Role Approver 1 & 2 has access to approving the transaction data. Once approve / reject, the task will not be present in the list anymore.
15. Hence, the project is ready to be tested. 
