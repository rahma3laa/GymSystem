using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
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

        #region Create Member

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost] //From Form
        public ActionResult CreateMember(CreateMemberViewModel createdMember)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("Data Invalid", "Check Data and Missing Fields");
                return View(nameof(Create), createdMember);
            }
            bool Result= _memberService.CreateMember(createdMember);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create , Check Phone and Email";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Member
        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));

            }
            var member=_memberService.GetMemberToUpdate(id);
            if(member is null)
            {
                TempData["ErrorMessage"] = " Member Not Found";
                return RedirectToAction(nameof(Index));

            }
            return View(member);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id , MemberToUpdateViewModel MemberToEdit)
        {
           if(!ModelState.IsValid)
                return View(nameof(Index));
           var Result=_memberService.UpdateMemberDetails(id, MemberToEdit);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Member  Failed To Updated ";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Member
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));

            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = " Member Not Found";
                return RedirectToAction(nameof(Index));

            }
            ViewBag.MemberId = id;
            ViewBag.MemberName=member.Name;
            return View();
        }

        [HttpPost] 
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var Result=_memberService.RemoveMember(id);
            if(Result)
               
               TempData["SuccessMessage"] = "Member Deleted Successfully";

            else
                TempData["ErrorMessage"] = " Member Not Found";
            return RedirectToAction(nameof(Index));

                             
            }
    }
        #endregion
}

