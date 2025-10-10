using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostiories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repostiories.Classes
{
    public class PlanRepository : IPlanReposatory
    {
        private readonly GymDbContext _dbContext;
        public PlanRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        public int Add(Plan plan)
        {
            _dbContext.Add(plan);   
            return _dbContext.SaveChanges();
        }

        public int Delete(Plan plan)
        {
           _dbContext.Plans.Remove(plan);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Plan> GetAll() => _dbContext.Plans.ToList();

        public Plan GetById(int Id) => _dbContext.Plans.Find(Id);

        public int Update(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return _dbContext.SaveChanges();
        }
    }
}
