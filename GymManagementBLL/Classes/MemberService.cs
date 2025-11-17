using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostiories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Classes
{
     public class MemberService : IMemberService
     {
       
        public MemberService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public readonly IUnitOfWork _UnitOfWork;
        public readonly IMapper _mapper;
        public bool CreateMember(CreateMemberViewModel createdMember)
        {

            try
            {
                
              if(IsEmailExsits(createdMember.Email) || IsPhoneExsits(createdMember.Phone)) return false;
              
                
                var member=_mapper.Map<CreateMemberViewModel , Member>(createdMember);
                _UnitOfWork.GetRepository<Member>().Add(member);
                return _UnitOfWork.SaveChanges() > 0;
               
            }
            catch (Exception )
            {
                return false;

            }

        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _UnitOfWork.GetRepository<Member>().GetAll();
            if (Members is null || !Members.Any()) return [];//Enumerable.Empty<MemberViewModel>();

         

            var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(Members);
            return MemberViewModels;
        }

        public HealthRecordViewModel? GetHealthRecordViewModel(int MemberId)
        {
            var MemberHealthRecord = _UnitOfWork.GetRepository<HealthRecord>().GetById(MemberId);

            
            if (MemberHealthRecord == null) return null;

            return _mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
            
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
         var Member= _UnitOfWork.GetRepository<Member>().GetById(MemberId);
          if(Member is null) return null;

            var ViewModel = _mapper.Map<MemberViewModel>(Member);

            //Active MemberShip
            var ActiveMemberShip = _UnitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemberId == MemberId && X.Status == "Active")
                                         .FirstOrDefault();

            if(ActiveMemberShip is not  null)
            {
                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate=ActiveMemberShip.EndDate.ToShortDateString();
            }

            var Plan = _UnitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
            ViewModel.PlanName = Plan?.Name;

            return ViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _UnitOfWork.GetRepository<Member>().GetById(MemberId);
            if(Member is null) return null;

            return _mapper.Map<MemberToUpdateViewModel>(Member);
        }

        public bool RemoveMember(int MemberId)
        {
            var MemberRepo = _UnitOfWork.GetRepository<Member>();
            var Member = MemberRepo.GetById(MemberId);
            if(Member is null) return false;

            var HasActiveMemberSessions = _UnitOfWork.GetRepository<MemberSession>()
                      .GetAll(X => X.MemberId == MemberId && X.Session.StartDate > DateTime.Now).Any();

            if (HasActiveMemberSessions) return false;

            var MemberShipRepo=_UnitOfWork.GetRepository<MemberShip>();
            var MemberShips = MemberShipRepo.GetAll(X=>X.MemberId == MemberId);
            try
            {
                if (MemberShips.Any())
                {
                    foreach(var membership in MemberShips)
                    {
                        MemberShipRepo.Delete(membership);
                    }
                }
                MemberRepo.Delete(Member) ;
                return _UnitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
        {
            try
            {
                
                if (IsEmailExsits(UpdatedMember.Email) || IsPhoneExsits(UpdatedMember.Phone)) return false;

                var Repo=_UnitOfWork.GetRepository<Member>();
                var Member = Repo.GetById(Id);
                if(Member is null) return false;

                _mapper.Map(UpdatedMember, Member);
                return _UnitOfWork.SaveChanges() > 0;            }
            catch
            {
                return false;
            }
        }

        #region Helper Method

        private bool IsEmailExsits(string email)
        {
            var Result = _UnitOfWork.GetRepository<Member>().GetAll(X=>X.Email == email).Any();
            return Result;
        }

        private bool IsPhoneExsits(string phone)
        {
            var Result = _UnitOfWork.GetRepository<Member>().GetAll(X=>X.Phone == phone).Any();
            return Result;
        }
        #endregion
    }
}
