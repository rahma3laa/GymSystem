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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly GymDbContext _dbContext;
        public CategoryRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(Category category)
        {
            _dbContext.Categories.Add(category);
            return _dbContext.SaveChanges();
        }

        public int Delete(Category category)
        {
           _dbContext.Categories.Remove(category);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Category> GetAll() => _dbContext.Categories.ToList();

        public Category GetById(int Id) => _dbContext.Categories.Find(Id);

        public int Update(Category category)
        {
           _dbContext.Categories.Update(category);
            return _dbContext.SaveChanges();
        }
    }
}
