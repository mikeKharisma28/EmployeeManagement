namespace EmployeeManagement.Models
{
    public class Position : BaseModel
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

    }
}
