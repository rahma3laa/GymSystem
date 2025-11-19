using GymManagementBLL.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }



        #region Index Get ALL Member
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View( members);
        
        
        }
        #endregion

        #region Get Member Data
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index)); 
            }
            var member = _memberService.GetMemberDetails(id);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index)); 
            }
            return View(member);

        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));

            }
               
            var HealthRecord = _memberService.GetHealthRecordViewModel(id);
            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "HealthRecord Not Found";
                return RedirectToAction(nameof(Index));
            }
               
            return View(HealthRecord);
        }
        #endregion

    }
}
