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
        public TrainerService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public bool CreateTrainer(CreateTrainerViewModel CreatedTrainer)
        {
            try
            {
                var Repo = _UnitOfWork.GetRepository<Trainer>();
                if(IsEmailExsits(CreatedTrainer.Email) || IsPhoneExsits(CreatedTrainer.Phone)) return false;

                var trainer = new Trainer()
                {
                    Name = CreatedTrainer.Name,
                    Email = CreatedTrainer.Email,
                    Phone = CreatedTrainer.Phone,
                    Specialties = CreatedTrainer.Specialities,
                    Gender = CreatedTrainer.Gender,
                    DateOfBirth = CreatedTrainer.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = CreatedTrainer.BuildingNumber,
                        City = CreatedTrainer.City,
                        Street = CreatedTrainer.Street,
                    }
                };
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

            return trainer.Select(T => new TrainerViewModel()
            {
                Id = T.Id,
                Name = T.Name,
                Email = T.Email,
                Phone = T.Phone,
                Specialization = T.Specialties.ToString()
            });

        }

        public TrainerViewModel? GetTrainerDetails(int TrainId)
        {
            var trainer = _UnitOfWork.GetRepository<Trainer>().GetById(TrainId);
            if(trainer ==  null) return null;

            return new TrainerViewModel()
            {
                Email = trainer.Email,
                Phone = trainer.Phone,
                Name = trainer.Name,
                Specialization=trainer.Specialties.ToString(),
            };
        }

        public UpdatedTrainerViewModel? GetTrainerToUpdate(int TrainId)
        {
            var trainer = _UnitOfWork.GetRepository<Trainer>().GetById(TrainId);
            if(trainer == null) return null;

            return new UpdatedTrainerViewModel()
            {
                TrainName = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Street = trainer.Address.Street,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Specialization = trainer.Specialties,
            };
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

            TrainerToUpdate.Email = UpdatedTrainer.Email;
            TrainerToUpdate.Phone = UpdatedTrainer.Phone;
            TrainerToUpdate.Address.BuildingNumber= UpdatedTrainer.BuildingNumber;
            TrainerToUpdate.Address.City = UpdatedTrainer.City;
            TrainerToUpdate.Address.Street= UpdatedTrainer.Street;
            TrainerToUpdate.Specialties = UpdatedTrainer.Specialization;
            TrainerToUpdate.UpdatedAt=DateTime.Now;
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
