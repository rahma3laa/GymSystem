using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostiories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Classes
{
    public class TrainerService : ITrainerService
    {
        public readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateTrainer(CreateTrainerViewModel CreatedTrainer)
        {
            try
            {
                var Repo = _UnitOfWork.GetRepository<Trainer>();
                if(IsEmailExsits(CreatedTrainer.Email) || IsPhoneExsits(CreatedTrainer.Phone)) return false;

                var trainer = _mapper.Map<Trainer>(CreatedTrainer);
                Repo.Add(trainer);
                return _UnitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainer = _UnitOfWork.GetRepository<Trainer>().GetAll();
            if (trainer == null || !trainer.Any()) return [];

            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainer);
        }

        public TrainerViewModel? GetTrainerDetails(int TrainId)
        {
            var trainer = _UnitOfWork.GetRepository<Trainer>().GetById(TrainId);
            if(trainer ==  null) return null;

            return _mapper.Map<TrainerViewModel>(trainer);
        }

        public UpdatedTrainerViewModel? GetTrainerToUpdate(int TrainId)
        {
            var trainer = _UnitOfWork.GetRepository<Trainer>().GetById(TrainId);
            if(trainer == null) return null;

            return _mapper.Map<UpdatedTrainerViewModel>(trainer);
        }

        public bool RemoveTrainer(int TrainId)
        {
            var Repo = _UnitOfWork.GetRepository<Trainer>();
            var TrainerToRemove=Repo.GetById(TrainId);
            if(TrainerToRemove == null ||HasActiveMemberShips(TrainId)) return false;

            Repo.Delete(TrainerToRemove);
            return _UnitOfWork.SaveChanges() > 0;
        }

        public bool UpdateTrainerDetails(UpdatedTrainerViewModel UpdatedTrainer, int TrainId)
        {
           var Repo= _UnitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate=Repo.GetById(TrainId);
            if(TrainerToUpdate is null || IsEmailExsits(UpdatedTrainer.Email) || IsPhoneExsits(UpdatedTrainer.Phone)) return false;

            _mapper.Map(UpdatedTrainer, TrainerToUpdate);
            Repo.Update(TrainerToUpdate);
            return _UnitOfWork.SaveChanges() > 0;

        }

        #region Helper Method

        private bool IsEmailExsits(string email)
        {
            var Result = _UnitOfWork.GetRepository<Trainer>().GetAll(X => X.Email == email).Any();
            return Result;
        }

        private bool IsPhoneExsits(string phone)
        {
            var Result = _UnitOfWork.GetRepository<Trainer>().GetAll(X => X.Phone == phone).Any();
            return Result;
        }

        private bool HasActiveMemberShips(int TrainerId)
        {
            var ActiveSessions = _UnitOfWork.GetRepository<Session>()
                .GetAll(X => X.TrainerId == TrainerId && X.StartDate > DateTime.Now).Any();
            return ActiveSessions;
        }
        #endregion
    }
}
