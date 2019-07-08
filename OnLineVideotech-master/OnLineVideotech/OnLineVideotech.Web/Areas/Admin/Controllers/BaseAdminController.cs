using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnLineVideotech.Web.Infrastructure;

namespace OnLineVideotech.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public abstract class BaseAdminController : Controller
    {
    }
}