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
    internal class MemberService : IMemberService
    {
       
        public MemberService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IUnitOfWork _UnitOfWork { get; }

        public bool CreateMember(CreateMemberViewModel createdMember)
        {

            try
            {
               
                //If one of them exists return false 
                if (IsPhoneExsits(createdMember.Phone) || IsEmailExsits(createdMember.Email)) return false;

                //if not add member and return true
                var member = new Member()
                {
                    Name = createdMember.Name,
                    Email = createdMember.Email,
                    Phone = createdMember.Phone,
                    DateOfBirth = createdMember.DateOfBirth,
                    Gender = createdMember.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createdMember.BuildingNumber,
                        City = createdMember.City,
                        Street = createdMember.Street,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createdMember.HealthRecordViewModel.Height,
                        weight = createdMember.HealthRecordViewModel.Weight,
                        BloodType = createdMember.HealthRecordViewModel.BloodType,
                        Note = createdMember.HealthRecordViewModel.Note,
                    }
                };
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

            #region Manual Mapping
            //var MemberViewModels = new List<MemberViewModel>();
            //foreach (var Member in Members)
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        Id = Member.Id,
            //        Name = Member.Name,
            //        Phone = Member.Phone,
            //        Email = Member.Email,
            //        Gender = Member.Gender.ToString(),
            //        Photo = Member.Photo,
            //    };
            //}
            //return MemberViewModels; 
            #endregion

            var MemberViewModels = Members.Select(X => new MemberViewModel
            {
                Id = X.Id,
                Name = X.Name,
                Photo = X.Photo,
                Gender = X.Gender.ToString(),
                Email = X.Email,
                Phone = X.Phone,
            });
            return MemberViewModels;

        }

        public HealthRecordViewModel? GetHealthRecordViewModel(int MemberId)
        {
            var MemberHealthRecord = _UnitOfWork.GetRepository<HealthRecord>().GetById(MemberId);

            
            if (MemberHealthRecord == null) return null;

            return new HealthRecordViewModel()
            {
                BloodType = MemberHealthRecord.BloodType,
                Height = MemberHealthRecord.Height,
                Weight = MemberHealthRecord.weight,
                Note = MemberHealthRecord.Note,
            };
            
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
         var Member= _UnitOfWork.GetRepository<Member>().GetById(MemberId);
          if(Member is null) return null;

            var ViewModel = new MemberViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString(),
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Address = $"{Member.Address.BuildingNumber} - {Member.Address.Street} - {Member.Address.City} ",
                Photo = Member.Photo,

            };

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

            return new MemberToUpdateViewModel()
            {
                Email = Member.Email,
                Phone = Member.Phone,
                Name = Member.Name,
                Photo = Member.Photo,
                BuildingNumber = Member.Address.BuildingNumber,
                City = Member.Address.City,
                Street = Member.Address.Street
            };
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

                Member.Email = UpdatedMember.Email;
                Member.Phone = UpdatedMember.Phone;

                Member.Address.BuildingNumber = UpdatedMember.BuildingNumber;
                Member.Address.City = UpdatedMember.City;
                Member.Address.Street = UpdatedMember.Street;
                Member.UpdatedAt = DateTime.Now;

                 Repo.Update(Member);
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
