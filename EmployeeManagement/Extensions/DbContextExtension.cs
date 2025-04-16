using EmployeeManagement.Models;

namespace EmployeeManagement.Extensions
{
    public static class DbContextExtension
    {
        public static void SoftDelete<T>(this AppDbContext context, T entity) where T : class, ISoftDelete
        {
            entity.IsDeleted = true;
            context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public static void SoftDelete<T>(this AppDbContext context, ICollection<T> entities) where T : class, ISoftDelete
        {
            foreach (var r in entities)
            {
                r.IsDeleted = true;
                context.Entry(entities).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
        }
    }
}
