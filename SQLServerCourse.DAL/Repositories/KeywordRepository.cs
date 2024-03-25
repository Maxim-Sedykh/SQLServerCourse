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
    public class KeywordRepository: IBaseRepository<Keyword>
    {
        private readonly CourseDbContext _db;

        public KeywordRepository(CourseDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Keyword entity)
        {
            await _db.Keywords.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Keyword entity)
        {
            _db.Keywords.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Keyword> Update(Keyword entity)
        {
            _db.Keywords.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<Keyword> GetAll()
        {
            return _db.Keywords;
        }
    }
}
