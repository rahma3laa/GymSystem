using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
    
    
            private readonly IPlanService _planService;

            public PlanController(IPlanService planService)
            {
                _planService = planService;
            }
            public ActionResult Index()
            {
                var plans = _planService.GetAllPlans();
                return View(plans);
            }
            public ActionResult Details(int id)
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Invalid Plan Id.";
                    return RedirectToAction(nameof(Index));
                }
                var plan = _planService.GetPlanById(id);
                if (plan == null)
                {
                    TempData["ErrorMessage"] = "Plan not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(plan);

            }
            public ActionResult Edit(int id)
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Invalid Plan Id.";
                    return RedirectToAction(nameof(Index));
                }
                var plan = _planService.GetPlanToUpdate(id);
                if (plan == null)
                {
                    TempData["ErrorMessage"] = "Plan Can Not Be Updated.";
                    return RedirectToAction(nameof(Index));
                }
                return View(plan);
            }
            [HttpPost]
            public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel planUpdate)
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("WrongData", "Check Missing Fields");
                    return View(planUpdate);
                }
                bool Result = _planService.UpdatePlan(id, planUpdate);
                if (Result)
                {
                    TempData["SuccessMessage"] = "Plan Updated Successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Plan Failed To Update , Name already exists";
                }
                return RedirectToAction(nameof(Index));

            }

            [HttpPost]
            public ActionResult Activate(int id)
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Invalid Plan Id.";
                    return RedirectToAction(nameof(Index));
                }
                bool Result = _planService.Activate(id);
                if (Result)
                {
                    TempData["SuccessMessage"] = "Plan Activation Status Changed Successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Plan Failed To Change Activation Status.";
                }
                return RedirectToAction(nameof(Index));
            }
        }
}
