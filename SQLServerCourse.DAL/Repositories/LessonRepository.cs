using Microsoft.EntityFrameworkCore;
using SQLServerCourse.DAL.Contexts;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Repositories
{
    public class LessonRepository: IBaseRepository<Lesson>
    {
        public readonly CourseDbContext _db;

        public LessonRepository(CourseDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Lesson entity)
        {
            await _db.Lessons.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Lesson entity)
        {
            _db.Lessons.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Lesson> Update(Lesson entity)
        {
            _db.Lessons.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<Lesson> GetAll()
        {
            return _db.Lessons;
        }
    }
}
