using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EmployeeManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "EmployeeManagement_";
            });

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"))
            );

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders().AddApiEndpoints();

            builder.Services.AddTransient<UserResolverServices>();
            builder.Services.AddScoped<IUserAccountServices, UserAccountServices>();
            builder.Services.AddScoped<ExternalAPIServices>();

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication().AddCookie();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
                        
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
