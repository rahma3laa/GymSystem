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
