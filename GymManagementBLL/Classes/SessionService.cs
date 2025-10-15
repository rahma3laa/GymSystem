using AutoMapper;
using GymManagementBLL.Services;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostiories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel CreatedSession)
        {
            try
            {
                if (!IsTrainedExist(CreatedSession.TrainerId)) return false;

                if (!IsCategoryExist(CreatedSession.CategoryId)) return false;

                if (!IsDateTimeValid(CreatedSession.StartDate, CreatedSession.EndDate)) return false;

                if (CreatedSession.Capacity > 25 || CreatedSession.Capacity < 0) return false;

                var SessionEntity = _mapper.Map<Session>(CreatedSession);

                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }catch(Exception ex)
            {
                Console.WriteLine($"Create Session Failed {ex}");
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!Sessions.Any()) return [];

            //    return Sessions.Select(S => new SessionViewModel
            //    {
            //        Id = S.Id,
            //        Description = S.Description,
            //        StartDate = S.StartDate,
            //        EndDate = S.EndDate,
            //        Capacity = S.Capacity,
            //        TrainerName = S.Trainer.Name,
            //        CategoryName = S.Category.CategoryName,
            //        AvailableStoles = S.Capacity - _unitOfWork.SessionRepository.GetCountOfBookSlots(S.Id),
            //    });
            //}

            var MappedSessions=_mapper.Map<IEnumerable<Session> , IEnumerable<SessionViewModel>>(Sessions);

            foreach (var Session in MappedSessions)
                Session.AvailableStoles = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookSlots(Session.Id);
            return MappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
           var Sessions = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (Sessions == null) return null;
            //return new SessionViewModel
            //{
            //    Description = Session.Description,
            //    TrainerName = Session.Trainer.Name,
            //    CategoryName = Session.Category.CategoryName,
            //    EndDate = Session.EndDate,
            //    StartDate = Session.StartDate,
            //    Capacity = Session.Capacity,
            //    AvailableStoles = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookSlots(Session.Id)
            //};

            var MappedSessions = _mapper.Map<Session, SessionViewModel>(Sessions);

            MappedSessions.AvailableStoles = MappedSessions.Capacity - _unitOfWork.SessionRepository.GetCountOfBookSlots(Sessions.Id);
            return MappedSessions;
        }

        #region Helper Method

        private bool IsTrainedExist(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }

        private bool IsCategoryExist(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime StartDate , DateTime EndDate)
        {
            return StartDate < EndDate;
        }
        #endregion

    }
}
