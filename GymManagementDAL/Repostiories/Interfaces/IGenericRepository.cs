using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repostiories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()  //Abstract Class Have No Constructor
    {
        IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null);

        TEntity? GetById(int Id);

        int Add(TEntity entity);
        int Update(TEntity entity);

        int Delete(TEntity entity);
    }

}
