using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class Employee : BaseModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string EmployeeNumber { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string PhotoUrl { get; set; }

        [Required]
        [ForeignKey("PositionId")]
        public Guid PositionId { get; set; }
        
        public virtual Position Position { get; set; }
    }
}
