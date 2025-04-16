using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace EmployeeManagement
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly UserResolverServices _userResolverServices;

        public AppDbContext(DbContextOptions options, UserResolverServices userResolverServices) : base(options)
        {
            _userResolverServices = userResolverServices;
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Position> Positions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().ToTable("AppUsers");
            builder.Entity<IdentityRole>().ToTable("AppRoles");
            builder.Entity<IdentityUserRole<string>>().ToTable("AppUserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AppUserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("AppUserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AppRoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("AppUserTokens");

            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(x => x.GetForeignKeys())
                .Where(x => !x.IsOwnership && x.DeleteBehavior == DeleteBehavior.Cascade);

            foreach(var foreignKey in cascadeFKs)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            var softDeleteEntities = typeof(ISoftDelete).Assembly.GetTypes()
                .Where(x => typeof(ISoftDelete).IsAssignableFrom(x) && x.IsClass && x.BaseType == null);

            foreach (var entity in softDeleteEntities)
            {
                builder.Entity(entity).HasQueryFilter(GenerateQueryFilterLambda(entity));
            }
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var added = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity)
                .ToList();

            foreach (var entity in added)
            {
                if (entity is IBaseModel)
                {
                    var track = entity as IBaseModel;
                    track.Id = Guid.NewGuid();
                    track.CreatedDate = DateTime.Now;
                    track.CreatedBy = _userResolverServices.GetUserName();
                }
            }

            var modified = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .ToList();

            foreach (var entity in modified)
            {
                if (entity is IBaseModel)
                {
                    var track = entity as IBaseModel;
                    track.UpdatedDate = DateTime.Now;
                    track.UpdatedBy = _userResolverServices.GetUserName();
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();

            var added = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity)
                .ToList();

            foreach (var entity in added)
            {
                if (entity is IBaseModel)
                {
                    var track = entity as IBaseModel;
                    track.Id = Guid.NewGuid();
                    track.CreatedDate = DateTime.Now;
                    track.CreatedBy = _userResolverServices.GetUserName();
                }
            }

            var modified = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .ToList();

            foreach (var entity in modified)
            {
                if (entity is IBaseModel)
                {
                    var track = entity as IBaseModel;
                    track.UpdatedDate = DateTime.Now;
                    track.UpdatedBy = _userResolverServices.GetUserName();
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        private LambdaExpression? GenerateQueryFilterLambda(Type type)
        {
            var parameter = Expression.Parameter(type, "x");
            var falseConstantValue = Expression.Constant(false);
            var propertyAccess = Expression.PropertyOrField(parameter, nameof(ISoftDelete.IsDeleted));
            var isFalseExpression = Expression.IsFalse(propertyAccess);
            var lambda = Expression.Lambda(isFalseExpression, parameter);

            return lambda;
        }
    }
}
