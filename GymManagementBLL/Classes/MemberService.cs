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
        public readonly IGenericRepository<Member> _MemberRepository;
        private readonly IGenericRepository<MemberShip> _memberShipRepository;
        private readonly IPlanReposatory _planReposatory;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;

        public MemberService(IGenericRepository<Member> MemberRepository , 
            IGenericRepository<MemberShip> MemberShipRepository , 
            IPlanReposatory planReposatory , IGenericRepository<HealthRecord> HealthRecordRepository)
        {
            _MemberRepository = MemberRepository;
            _memberShipRepository = MemberShipRepository;
            _planReposatory = planReposatory;
            _healthRecordRepository = HealthRecordRepository;
        }

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
                return _MemberRepository.Add(member) > 0; //True 
            }
            catch (Exception )
            {
                return false;

            }

        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _MemberRepository.GetAll();
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
            var MemberHealthRecord = _healthRecordRepository.GetById(MemberId);

            if(MemberHealthRecord == null) return null;

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
         var Member= _MemberRepository.GetById(MemberId);
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
            var ActiveMemberShip = _memberShipRepository.GetAll(X => X.MemberId == MemberId && X.Status == "Active")
                                         .FirstOrDefault();

            if(ActiveMemberShip is not  null)
            {
                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate=ActiveMemberShip.EndDate.ToShortDateString();
            }

            var Plan = _planReposatory.GetById(ActiveMemberShip.PlanId);
            ViewModel.PlanName = Plan?.Name;

            return ViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _MemberRepository.GetById(MemberId);
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

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
        {
            try
            {
                
                if (IsEmailExsits(UpdatedMember.Email) || IsPhoneExsits(UpdatedMember.Phone)) return false;

                var Member = _MemberRepository.GetById(Id);
                if(Member is null) return false;

                Member.Email = UpdatedMember.Email;
                Member.Phone = UpdatedMember.Phone;

                Member.Address.BuildingNumber = UpdatedMember.BuildingNumber;
                Member.Address.City = UpdatedMember.City;
                Member.Address.Street = UpdatedMember.Street;
                Member.UpdatedAt = DateTime.Now;

                return _MemberRepository.Update(Member) > 0;
               
            }
            catch
            {
                return false;
            }
        }

        #region Helper Method

        private bool IsEmailExsits(string email)
        {
            var Result = _MemberRepository.GetAll(X=>X.Email == email).Any();
            return Result;
        }

        private bool IsPhoneExsits(string phone)
        {
            var Result = _MemberRepository.GetAll(X=>X.Phone == phone).Any();
            return Result;
        }
        #endregion
    }
}
