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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        private readonly GymDbContext _dbContext;
        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        public void Add(TEntity entity) => _dbContext.Set<TEntity>().Add(entity);
        //_dbContext.Add(entity);


        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition is not null)
                return _dbContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();
            else
                return _dbContext.Set<TEntity>().AsNoTracking().ToList();
        }

        public TEntity? GetById(int Id) => _dbContext.Set<TEntity>().Find(Id);

        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
        
            
         
    }
}
