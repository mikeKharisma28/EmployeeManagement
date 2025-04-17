using EmployeeManagement.DTOs.Account;

namespace EmployeeManagement.Services
{
    public interface IUserAccountServices
    {
        Task<bool> Register(RegisterDto insertEmployeeDto);

        Task<bool> AssignUserRole(string userName, string roleName);
    }
}