using EmployeeManagement.DTOs.Account;
using EmployeeManagement.DTOs.Employee;

namespace EmployeeManagement.Services
{
    public interface IUserAccountServices
    {
        Task<bool> Register(RegisterDto insertEmployeeDto);

        Task<bool> Register(InsertDto insertEmployeeDto);

        Task<bool> AssignUserRole(string userName, string roleName);
    }
}