using Microsoft.AspNetCore.Mvc;
using OnLineVideotech.Services.Interfaces;
using OnLineVideotech.Services.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnLineVideotech.Web.Areas.Admin.Controllers
{
    public class HistoryAdminController : BaseAdminController
    {
        private readonly IHistoryService historyService;

        public HistoryAdminController(IHistoryService historyService)
        {
            this.historyService = historyService;
        }

        public async Task<IActionResult> History()
        {
            List<HistoryServiceModel> histories = await this.historyService.GetHistory();

            return View(histories);
        }

        [HttpPost]
        public async Task<IActionResult> History(HistoryServiceModel model)
        {
            List<HistoryServiceModel> histories = await this.historyService.GetHistory();

            return View(histories);
        }
    }
}
