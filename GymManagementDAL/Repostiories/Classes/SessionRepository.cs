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

       

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _dbContext.Sessions.Include(X => X.Trainer)
                                       .Include(X=>X.Category)
                                       .ToList();
        }

        public int GetCountOfBookSlots(int SessionId)
        {
            return _dbContext.MembersSessions.Count(X => X.SessionId == SessionId);
        }
    }
}
