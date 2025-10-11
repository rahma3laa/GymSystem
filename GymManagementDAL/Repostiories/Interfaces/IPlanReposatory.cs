using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repostiories.Interfaces
{
    public interface IPlanReposatory
    {
        IEnumerable<Plan> GetAll();

        Plan? GetById(int Id);

        int Update(Plan plan);

       
    }
}
