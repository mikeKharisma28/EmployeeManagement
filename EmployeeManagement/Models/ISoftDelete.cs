namespace EmployeeManagement.Models
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
