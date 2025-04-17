using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole(string roleName) : base(roleName)
        {
        }

        public Guid PositionId { get; set; }
    }
}
