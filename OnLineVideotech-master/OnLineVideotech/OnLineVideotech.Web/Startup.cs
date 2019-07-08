using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnLineVideotech.Data;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Web.Infrastructure.Extensions;
using AutoMapper;
using OnLineVideotech.Services.Admin.Interfaces;
using OnLineVideotech.Services.Admin.Implementations;
using OnLineVideotech.Services.Implementations;
using OnLineVideotech.Services.Interfaces;

namespace OnLineVideotech.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<OnLineVideotechDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
            })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<OnLineVideotechDbContext>()
                .AddDefaultTokenProviders();


            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IMovieManagementService, MovieManagementService>();
            services.AddTransient<IAdminUserService, AdminUserService>();
            services.AddTransient<IPriceService, PriceService>();
            services.AddTransient<IGenreService, GenreService>();
            services.AddTransient<IGenreMovieService, GenreMovieService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IUserBalanceService, UserBalanceService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IHistoryService, HistoryService>();

            services.AddAutoMapper();

            services.AddMemoryCache();

            services.AddMvc(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);           
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDatabaseMigration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Movie/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Movie}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Movie}/{action=Index}/{id?}");
            });
        }
    }
}