using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interface
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();  //GetAll

        bool CreateMember(CreateMemberViewModel createdMember); //Create 

        //Get
        MemberViewModel? GetMemberDetails(int MemberId);

        HealthRecordViewModel? GetHealthRecordViewModel(int MemberId);

        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);

        bool UpdateMemberDetails(int Id , MemberToUpdateViewModel UpdatedMember );
    }
}
