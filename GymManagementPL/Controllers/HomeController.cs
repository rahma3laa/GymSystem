using GymManagementBLL.Services.Interface;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        public readonly IAnalyticsService _analyticsService;
        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

      

        public ActionResult Index()
        {
            var Data = _analyticsService.GetAnalyticsData();

            return View(Data);
        }

    
    }
}

