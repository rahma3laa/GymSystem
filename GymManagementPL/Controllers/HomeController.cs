using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        //ActionResult
        public ViewResult Index()
        {
          return View();

        }

        public JsonResult Trainers()
        {
            var Trainers = new List<Trainer>()
            {
                new Trainer(){ Name ="Rahma" , Phone ="010060"},
                new Trainer(){ Name ="Ali" , Phone ="01006096175"},
            };
            return Json(Trainers);
        }

        public RedirectResult Redirect()
        {
            return Redirect("https://www.bing.com/search?q=download+github+desktop&gs_lcrp=EgRlZGdlKgcIARBFGMIDMgcIABBFGMIDMgcIARBFGMIDMgcIAhBFGMIDMgcIAxBFGMIDMgcIBBBFGMIDMgcIBRBFGMIDMgcIBhBFGMIDMgcIBxBFGMID0gENNTgyOTgzNzYwajBqMagCCLACAQ&FORM=ANNTA1&ucpdpc=UCPD&adppc=EDGEESS&PC=U531");
        }

        public ContentResult Content()
        {
            return Content("<h1>Hello From Gym Management System</h1>" , "text/html" );
        }

        public FileResult DownloadFile()
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "site.css");
            var FileBytes=System.IO.File.ReadAllBytes(FilePath);
            return File(FileBytes ,"text/css" , "DownloadableSite.css"  );
        }

        public EmptyResult EmptyResult()
        {
            return new EmptyResult();
        }
    }
}

