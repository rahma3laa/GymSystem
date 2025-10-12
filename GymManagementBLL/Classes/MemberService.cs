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
        public MemberService(IGenericRepository<Member> MemberRepository)
        {
            _MemberRepository = MemberRepository;
        }

        public bool CreateMember(CreateMemberViewModel createdMember)
        {

            try
            {
                //Check Phone Is Exist
                var emailExists = _MemberRepository.GetAll(X => X.Email == createdMember.Email).Any();
                //if (emailExists) return false;

                //Check Email Is Exist
                var phoneCheck = _MemberRepository.GetAll(X => X.Phone == createdMember.Phone).Any();

                //If one of them exists return false 
                if (emailExists || phoneCheck) return false;

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
        
    }
}
