using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnLineVideotech.Data;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.Interfaces;
using OnLineVideotech.Services.Admin.ServiceModels;

namespace OnLineVideotech.Services.Admin.Implementations
{
    public class AdminUserService : BaseService, IBaseService, IAdminUserService
    {
        private readonly IRoleService roleService;

        public AdminUserService(OnLineVideotechDbContext db, IRoleService roleService) : base(db)
        {
            this.roleService = roleService;
        }

        public async Task<IEnumerable<AdminUserListingServiceModel>> AllAsync()
        {
            IEnumerable<User> users = await this.Db.Users.ToListAsync(); ;
            List<AdminUserListingServiceModel> userModels = new List<AdminUserListingServiceModel>();

            string role = null;

            foreach (User user in users)
            {
                role = await this.roleService.GetUserRole(user.Id);

                AdminUserListingServiceModel userModel = new AdminUserListingServiceModel
                {
                    Email = user.UserName,
                    Id = user.Id,
                    Role = role
                };

                userModels.Add(userModel);
            }

            return userModels;
        }
    }
}