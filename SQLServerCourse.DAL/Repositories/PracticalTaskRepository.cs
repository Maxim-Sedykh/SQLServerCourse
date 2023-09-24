using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Repositories
{
    public class PracticalTaskRepository : IBaseRepository<PracticalTask>
    {
        private readonly ApplicationDbContext _db;

        public PracticalTaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(PracticalTask entity)
        {
            await _db.PracticalTasks.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(PracticalTask entity)
        {
            _db.PracticalTasks.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<PracticalTask> Update(PracticalTask entity)
        {
            _db.PracticalTasks.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<PracticalTask> GetAll()
        {
            return _db.PracticalTasks;
        }
    }
}
