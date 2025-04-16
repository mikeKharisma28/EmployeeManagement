using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class AppUser : IdentityUser
    {
        public string EmployeeNumber { get; set; }
    }
}
