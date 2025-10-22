using GymManagementDAL.Entities;
using GymManagementDAL.Repostiories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repostiories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>

    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory(Func<Session, bool>? condition = null);
        Session? GetSessionWithTrainerAndCategory(int SessionId);

        int GetCountOfBookedSlots(int SessionId);
    }

}
