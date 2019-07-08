using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnLineVideotech.Data;
using OnLineVideotech.Data.Models;
using System.Threading.Tasks;

namespace OnLineVideotech.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<OnLineVideotechDbContext>().Database.Migrate();

                UserManager<User> userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                RoleManager<Role> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

                Task.Run(async () =>
                {
                    string adminName = GlobalConstants.AdministratorRole;

                    string[] roles = new[]
                    {
                        adminName,
                        GlobalConstants.RegularUser,
                        GlobalConstants.SuperUser
                    };

                    foreach (var role in roles)
                    {
                        bool roleExists = await roleManager.RoleExistsAsync(role);

                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new Role
                            {
                                Name = role,
                            });
                        }
                    }

                    string adminEmail = "admin@mysite.com";
                    User adminUser = await userManager.FindByEmailAsync(adminEmail);                 

                    if (adminUser == null)
                    {
                        adminUser = new User
                        {
                            Email = adminEmail,
                            UserName = adminEmail
                        };

                        await userManager.CreateAsync(adminUser, "admin12");

                        await userManager.AddToRoleAsync(adminUser, adminName);
                    }
                })
                .Wait();
            }

            return app;
        }
    }
}