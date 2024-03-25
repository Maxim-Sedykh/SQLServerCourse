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
    public class QueryWordRepository: IBaseRepository<QueryWord>
    {
        private readonly CourseDbContext _db;

        public QueryWordRepository(CourseDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(QueryWord entity)
        {
            await _db.QueryWords.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(QueryWord entity)
        {
            _db.QueryWords.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<QueryWord> Update(QueryWord entity)
        {
            _db.QueryWords.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<QueryWord> GetAll()
        {
            return _db.QueryWords;
        }
    }
}
