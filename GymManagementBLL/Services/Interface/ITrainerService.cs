using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interface
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();

        bool CreateTrainer(CreateTrainerViewModel CreatedTrainer);
        TrainerViewModel? GetTrainerDetails(int TrainId);

        UpdatedTrainerViewModel? GetTrainerToUpdate(int TrainId);

        bool UpdateTrainerDetails(UpdatedTrainerViewModel UpdatedTrainer , int TrainId);

        bool RemoveTrainer(int TrainId);
    }
}
