using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Admin.Interfaces;
using OnLineVideotech.Services.Admin.ServiceModels;
using OnLineVideotech.Web.Areas.Admin.Models.Users;
using OnLineVideotech.Web.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnLineVideotech.Web.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        private readonly IAdminUserService users;
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(IAdminUserService users,
            RoleManager<Role> roleManager,
            UserManager<User> userManager)
        {
            this.users = users;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<AdminUserListingServiceModel> users = await this.users.AllAsync();

            IEnumerable<SelectListItem> roles = await this.roleManager
                .Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                })
                .ToListAsync();

            AdminUserListingsViewModel userRoles = new AdminUserListingsViewModel
            {
                Users = users,
                Roles = roles
            };

            return View(userRoles);
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(AddUserToRoleFormModel model)
        {
            bool roleExists = await this.roleManager.RoleExistsAsync(model.Role);
            User user = await this.userManager.FindByIdAsync(model.UserId);
            bool userExists = user != null;

            if (!roleExists || !userExists)
            {
                ModelState.AddModelError(string.Empty, "Invalid identity details");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            IList<string> roles = await this.userManager.GetRolesAsync(user);

            await this.userManager.RemoveFromRolesAsync(user, roles);
            await this.userManager.AddToRoleAsync(user, model.Role);

            TempData.AddSuccessMessage($"User {user.UserName} successfully added to {model.Role} role");

            return RedirectToAction(nameof(Index));
        }
    }
}