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
    internal class MemberRepository : IMemberRepository
    {
        // private readonly GymDbContext _dbContext = new GymDbContext();

        private readonly GymDbContext _dbContext;

        //Ask CLR To inject Object from GymDbcontext
        public MemberRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GymDbContext DbContext { get; }

        public int? Add(Member member)
        {
            _dbContext.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int Id)
        {
            var Member = _dbContext.Members.Find(Id);
            if (Member is not null) return 0;

            _dbContext.Members.Remove(Member);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Member> GetAll()  =>  _dbContext.Members.ToList();

        public Member? GetById(int id)
        {
            return _dbContext.Members.Find(id);
        }

        public int Update(Member member)
        {
           _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();
        }
    }
}
