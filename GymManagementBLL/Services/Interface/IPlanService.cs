using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interface
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();

        PlanViewModel? GetPlanById(int PlanId);

        UpdatePlanViewModel? GetPlanToUpdate(int PlanId);

        bool UpdatePlan(int PlanId, UpdatePlanViewModel updatePlan);

        bool ToggleStatus(int PlanId);
        bool Activate(int id);

    }
}
