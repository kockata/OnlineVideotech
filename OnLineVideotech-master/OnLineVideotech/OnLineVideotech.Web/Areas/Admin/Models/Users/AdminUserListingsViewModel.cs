using Microsoft.AspNetCore.Mvc.Rendering;
using OnLineVideotech.Services.Admin.ServiceModels;
using System.Collections.Generic;

namespace OnLineVideotech.Web.Areas.Admin.Models.Users
{
    public class AdminUserListingsViewModel
    {
        public IEnumerable<AdminUserListingServiceModel> Users { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public string UserRole { get; set; }
    }
}