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
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();
        Session? GetById(int sessionId);
        int GetCountOfBookSlots(int SessionId);

        Session? GetSessionWithTrainerAndCategory(int sessionId);
        void Update(Session session);
    }

}
