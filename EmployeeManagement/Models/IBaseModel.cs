namespace EmployeeManagement.Models
{
    public interface IBaseModel
    {
        Guid Id { get; set; }

        DateTime CreatedDate { get; set; }

        string? CreatedBy { get; set; }

        DateTime UpdatedDate { get; set; }

        string? UpdatedBy { get; set; }
    }
}
