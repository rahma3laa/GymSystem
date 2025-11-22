using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostiories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        public readonly IUnitOfWork _unitOfWork;
        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        public AnalyticsViewModel GetAnalyticsData()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAll();
            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<MemberShip>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Session>().GetAll().Count(),
                OngoingSessions = Sessions.Count(X => X.StartDate > DateTime.Now),
                UpcomingSessions = Sessions.Count(X => X.StartDate <= DateTime.Now && X.EndDate >= DateTime.Now),
                CompletedSessions = Sessions.Count(X => X.StartDate < DateTime.Now)
            };

        }
    }
}
