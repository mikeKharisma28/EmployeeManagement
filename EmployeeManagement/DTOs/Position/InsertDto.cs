namespace EmployeeManagement.DTOs.Position
{
    public class InsertDto
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
