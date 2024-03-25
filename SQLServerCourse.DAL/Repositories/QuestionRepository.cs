using SQLServerCourse.DAL.Contexts;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Repositories
{
    public class QuestionRepository: IBaseRepository<Question>
    {
        private readonly CourseDbContext _db;

        public QuestionRepository(CourseDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Question entity)
        {
            await _db.Questions.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Question entity)
        {
            _db.Questions.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Question> Update(Question entity)
        {
            _db.Questions.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<Question> GetAll()
        {
            return _db.Questions;
        }
    }
}
