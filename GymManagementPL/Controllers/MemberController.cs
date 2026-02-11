using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index(int id)
        {
            //return RedirectToAction(nameof(GetMember));
            return RedirectToRoute("Trainers" , new { action  = "GetTrainers" } );
        }

        public ActionResult GetMember()
        {
            return View();
        }
        public ActionResult CreateMember()
        {
            return View();
        }
    }
}
