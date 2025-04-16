namespace EmployeeManagement.Services
{
    public class UserResolverServices
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserResolverServices(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string? GetUserName()
        {
            return _contextAccessor.HttpContext?.User?.Identity?.Name;
        }
    }
}
