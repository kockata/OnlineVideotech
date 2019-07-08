using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.Interfaces;
using OnLineVideotech.Services.ServiceModels;
using OnLineVideotech.Web.Infrastructure.Extensions;
using OnLineVideotech.Web.Models;
using System.Threading.Tasks;

namespace OnLineVideotech.Web.Controllers
{
    public class UserBalanceController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IUserBalanceService userBalanceService;

        public UserBalanceController(UserManager<User> userManager, IUserBalanceService userBalanceService)
        {
            this.userManager = userManager;
            this.userBalanceService = userBalanceService;
        }

        [Authorize]
        public async Task<IActionResult> Balance()
        {
            User user = await userManager.GetUserAsync(HttpContext.User);          

            UserBalanceServiceModel userBalanceModel = new UserBalanceServiceModel();

            userBalanceModel = this.userBalanceService.GetUserBalance(user.Id);

            return View(userBalanceModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Balance(UserBalanceServiceModel userBalanceModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userBalanceModel);
            }

            User user = await userManager.GetUserAsync(HttpContext.User);

            await this.userBalanceService.AddAmount(userBalanceModel, user.Id);

            TempData.AddSuccessMessage($"Successfully added {userBalanceModel.Balance} BGN to your account");

            return RedirectToAction(nameof(Balance));
        }
    }
}