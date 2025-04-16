namespace EmployeeManagement.Models
{
    public class BaseModel : IBaseModel, ISoftDelete
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
