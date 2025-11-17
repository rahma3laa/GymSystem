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
                return RedirectToAction(nameof(Index));
            var member = _memberService.GetMemberDetails(id);
            if(member is null)
                return RedirectToAction(nameof(Index));
            return View(member);

        }

        #endregion
    }
}
