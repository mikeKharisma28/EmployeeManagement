namespace EmployeeManagement.DTOs.Employee
{
    public class UpdateDto
    {
        public required string FullName { get; set; }

        public required string EmployeeNumber { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid PositionId { get; set; }

        public IFormFile filePhoto { get; set; }
    }
}
