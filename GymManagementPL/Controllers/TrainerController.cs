using GymManagementBLL.Classes;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        public readonly ITrainerService _trainerService;
        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region Get All Trainers

        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        #endregion

        #region Create Trainer
        //http[get]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Data Missed", "Check Data and Missing Fields");
                return View(nameof(Create), model);
            }
            bool Result = _trainerService.CreateTrainer(model);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Create , Check Phone and Email";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details Trainer
        
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id";
                return RedirectToAction(nameof(Index));

            }

            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }
        #endregion

        #region Edit Trainer
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));

            }
            var trainer = _trainerService.GetTrainerToUpdate(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = " Trainer Not Found";
                return RedirectToAction(nameof(Index));

            }
            return View(trainer);
        }

        [HttpPost]
        public ActionResult TrainerEdit([FromRoute] int id, MemberToUpdateViewModel MemberToEdit)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index));
           // var Result = _trainerService.UpdateTrainerDetails(id, MemberToEdit);
            //if ()
            //{
            //    TempData["SuccessMessage"] = "Trainer Updated Successfully";

            //}
            else
            {
                TempData["ErrorMessage"] = " Trainer Failed To Updated ";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id.";
                return RedirectToAction(nameof(Index));
            }
            var member = _trainerService.GetTrainerDetails(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            return View();

        }
        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Trainer Id.";
                return RedirectToAction(nameof(Index));
            }
            bool Result = _trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Delete.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
