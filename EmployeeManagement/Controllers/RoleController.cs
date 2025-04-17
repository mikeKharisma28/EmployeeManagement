using EmployeeManagement.DTOs.Account;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/role")]
    [ApiController]
    //[Authorize(Roles = "Super Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserAccountServices _userAccountServices;

        public RoleController(RoleManager<IdentityRole> roleManager, IUserAccountServices userAccountServices)
        {
            _roleManager = roleManager;
            _userAccountServices = userAccountServices;
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new AppRole(role));
                if (result.Succeeded)
                {
                    return Ok(new { message = "Role added successfully" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Role already exists");
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
        {
            var isSucceeded = await _userAccountServices.AssignUserRole(model.Username, model.RoleName);
            if (isSucceeded)
            {
                return Ok(new { message = "Role assigned successfully" });
            }

            return BadRequest();
        }
    }
}
