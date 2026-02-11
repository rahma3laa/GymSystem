using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostiories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Classes
{
    internal class PlanService : IPlanService
    {
        public readonly IUnitOfWork _UnitOfWork;
        public PlanService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }



        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _UnitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || Plans.Any() == false) return [];

            return Plans.Select(P => new PlanViewModel()
            {
                Description = P.Description,
                DurationDays = P.DurationDays,
                Name = P.Name,
                Id = P.Id,
                IsActive = P.IsActive,
                Price = P.Price
            });
        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
            var plan = _UnitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null) return null;
            return new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive,
                Price = plan.Price,
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var plan = _UnitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive == false || HasActiveMemberShips(PlanId)) return null;

            return new UpdatePlanViewModel()
            {

                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                PlanName = plan.Name,
            };
        }

        //soft delete update 
        public bool ToggleStatus(int PlanId)
        {
            var Repo = _UnitOfWork.GetRepository<Plan>();
            var plan = _UnitOfWork.GetRepository<Plan>().GetById(PlanId);

            if(plan == null || HasActiveMemberShips(PlanId)) return false;

            plan.IsActive = plan.IsActive == true ? false : true;
            plan.UpdatedAt= DateTime.Now;

            try
            {
                Repo.Update(plan);
                return _UnitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;

            }
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatePlan)
        {
            var Plan = _UnitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan == null || HasActiveMemberShips(PlanId)) return false;

           
            try
            {
                (Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatedAt) =
                (updatePlan.Description, updatePlan.Price, updatePlan.DurationDays, DateTime.Now);

                _UnitOfWork.GetRepository<Plan>().Update(Plan);
                return _UnitOfWork.SaveChanges() > 0; 
            }
            catch
            {
                return false;
            }
        }

        #region Helper

        private bool HasActiveMemberShips(int PlanId)
        {
            var ActiveMemberships = _UnitOfWork.GetRepository<MemberShip>()
                .GetAll(X => X.PlanId == PlanId && X.Status == "Active");
            return ActiveMemberships.Any();
        }
        #endregion
    }
}
