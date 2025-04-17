using EmployeeManagement.DTOs.Account;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Services
{
    public class UserAccountServices : IUserAccountServices
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserAccountServices(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> Register(RegisterDto insertEmployeeDto)
        {
            var user = new AppUser()
            {
                UserName = insertEmployeeDto.Email,
                Email = insertEmployeeDto.Email,
                EmployeeNumber = insertEmployeeDto.EmployeeNumber
            };
            var result = await _userManager.CreateAsync(user, insertEmployeeDto.Password);

            if (result.Succeeded)
            {
                return result.Succeeded;
            }
            else
            {
                throw new Exception(result.Errors.ToString());
            }
        }

        public async Task<bool> AssignUserRole(string userName, string roleName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new Exception($"{userName} is not registered.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return result.Succeeded;
            }
            else
            {
                throw new Exception(result.Errors.ToString());
            }
        }
    }
}
