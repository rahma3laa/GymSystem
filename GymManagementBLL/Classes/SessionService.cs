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

        public bool CreateSession(CreateSessionViewModel session)
        {
            try
            {
                if (
                    !IsTrainedExist(session.TrainerId)
                    || !IsCategoryExist(session.CategoryId)
                    || !IsDateTimeValid(session.StartDate, session.EndDate)
                )
                    return false;
                // Business Rule
                if (session.Capacity > 25 || session.Capacity < 0)
                    return false;

                // Info to be noted
                var mappedSession = _mapper.Map<Session>(session);

                _unitOfWork.SessionRepository.Add(mappedSession);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAll();
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
                Session.AvailableStoles = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id);
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

            MappedSessions.AvailableStoles = MappedSessions.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Sessions.Id);
            return MappedSessions;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetById(sessionId);

            if (!IsSessionAvailableForUpdating(Session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(Session);


            
        }

     

        public bool UpdateSession(UpdateSessionViewModel UpdatedSession, int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if(!IsSessionAvailableForUpdating(Session!)) return false;
                if(!IsTrainedExist(UpdatedSession.TrainerId)) return false;

                if (!IsDateTimeValid(UpdatedSession.StartDate, UpdatedSession.EndDate)) return false;
                _mapper.Map(UpdatedSession, Session);
                Session!.UpdatedAt = DateTime.Now;

                _unitOfWork.SessionRepository.Update(Session);
                return _unitOfWork.SaveChanges() > 0;

            }catch (Exception ex)
            {
                Console.WriteLine($"Updated Session Failed {ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if(!IsSessionAvailableForRemoving(Session!)) return false;

                _unitOfWork.SessionRepository.Delete(Session);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deleted Session Failed {ex}");
                return false;
            }
        }

        #region Helper Method
        private bool IsSessionAvailableForRemoving(Session session)
        {
            if (session is null) return false;

            // if Started
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            //if Is Upcoming
            if(session.StartDate > DateTime.Now) return false;
            // If Has Active Booking
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking) return false;

            return true;
        }

        private bool IsSessionAvailableForUpdating(Session session)
        {
            if(session is null) return false;
            // if Completed
            if(session.EndDate <DateTime.Now) return false;

            // if Started
            if(session.StartDate <= DateTime.Now) return false;

            // If Has Active Booking
            var HasActiveBooking=_unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if(HasActiveBooking) return false;

            return true;
        }
     

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
