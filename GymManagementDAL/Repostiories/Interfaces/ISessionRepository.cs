using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repostiories.Interfaces
{
    public interface ISessionRepository
    {
        //Get All

        IEnumerable<Session> GetAll();

        //Get By Id 

        Session GetById(int id);

        //Create 
        int Add(Session session);

        int Update(Session session);

        int Delete(Session session);
    }
}
