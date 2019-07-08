using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnLineVideotech.Data;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Admin.Implementations
{
    public class RoleService : BaseService, IBaseService, IRoleService
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;

        public RoleService(
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            OnLineVideotechDbContext db) : base(db)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            IEnumerable<Role> roles = await this.roleManager.Roles.ToListAsync();

            return roles;
        }

        public async Task<Role> FindRole(string roleId)
        {
            return  await this.roleManager.Roles.SingleAsync(x => x.Id == roleId);
        }

        public async Task<string> GetUserRole(string userId)
        {
            User user = await this.userManager.FindByIdAsync(userId);
            IList<string> roles = await this.userManager.GetRolesAsync(user);

            return roles.SingleOrDefault();
        }
    }
}