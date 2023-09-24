using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Repositories
{
    public class TaskAnswerRepository : IBaseRepository<TaskAnswer>
    {
        private readonly ApplicationDbContext _db;

        public TaskAnswerRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(TaskAnswer entity)
        {
            await _db.TaskAnswers.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(TaskAnswer entity)
        {
            _db.TaskAnswers.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<TaskAnswer> Update(TaskAnswer entity)
        {
            _db.TaskAnswers.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<TaskAnswer> GetAll()
        {
            return _db.TaskAnswers;
        }
    }
}
