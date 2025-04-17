namespace EmployeeManagement.DTOs.Position
{
    public class UpdateDto
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
