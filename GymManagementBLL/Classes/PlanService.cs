using AutoMapper;
using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostiories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Classes
{
    internal class PlanService : IPlanService
    {
        public readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork  , IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
           _mapper = mapper;
        }



        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _UnitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || Plans.Any() == false) return [];

            return _mapper.Map<IEnumerable<PlanViewModel>>(Plans);
        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
            var plan = _UnitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null) return null;
            return _mapper.Map<PlanViewModel>(plan);
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var plan = _UnitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive == false || HasActiveMemberShips(PlanId)) return null;

            return _mapper.Map<UpdatePlanViewModel>(plan);
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
            var repo = _UnitOfWork.GetRepository<Plan>();
            var plan = repo.GetById(PlanId);
            if (plan == null || HasActiveMemberShips(PlanId)) return false;

           
            try
            {
                _mapper.Map(updatePlan, plan);
                repo.Update(plan);
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
