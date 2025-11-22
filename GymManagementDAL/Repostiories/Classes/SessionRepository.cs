using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repostiories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repostiories.Classes
{
    public class SessionRepository :GenericRepository<Session> ,  ISessionRepository
    {
        public readonly GymDbContext _dbContext;
        public SessionRepository(GymDbContext dbContext) :base(dbContext)
        {
            _dbContext = dbContext;
        }

        public int CountOfBookingSlots(int sessionId)
        {
            return _dbContext.MembersSessions.Count(S => S.SessionId == sessionId);
        }
      

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory() =>
           _dbContext.Sessions.Include(S => S.Category).Include(S => S.Trainer).ToList();

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory(Func<Session, bool>? condition = null)
        {
            throw new NotImplementedException();
        }

        public int GetCountOfBookedSlots(int SessionId)
        {
            throw new NotImplementedException();
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _dbContext
                .Sessions.Include(S => S.Trainer)
                .Include(S => S.Category)
                .FirstOrDefault(S => S.Id == sessionId);
        }
    }
}
