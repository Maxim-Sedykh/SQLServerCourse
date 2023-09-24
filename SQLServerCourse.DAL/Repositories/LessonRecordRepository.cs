using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Repositories
{
    public class LessonRecordRepository: IBaseRepository<LessonRecord>
    {
        private readonly ApplicationDbContext _db;

        public LessonRecordRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(LessonRecord entity)
        {
            await _db.LessonRecords.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(LessonRecord entity)
        {
            _db.LessonRecords.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<LessonRecord> Update(LessonRecord entity)
        {
            _db.LessonRecords.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<LessonRecord> GetAll()
        {
           return _db.LessonRecords;
        }
    }
}   
