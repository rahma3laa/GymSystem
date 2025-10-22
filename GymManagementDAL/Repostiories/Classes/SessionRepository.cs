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


        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory(Func<Session, bool>? condition = null)
        {
            if (condition is null)
                return _dbContext.Sessions.Include(X => X.SessionTrainer)
                    .Include(X => X.SessionCategory)
                    .ToList();
            else
                return _dbContext.Sessions.Include(X => X.SessionTrainer)
                    .Include(X => X.SessionCategory)
                    .Where(condition).ToList();
        }

        public int GetCountOfBookedSlots(int SessionId)
        {
            return _dbContext.MembersSessions.Where(X => X.SessionId == SessionId).Count();
        }

        public Session? GetSessionWithTrainerAndCategory(int SessionId)
        {
            return _dbContext.Sessions.Include(X => X.SessionTrainer)
                                      .Include(X => X.SessionCategory).FirstOrDefault(X => X.Id == SessionId);
        }
    }
}
