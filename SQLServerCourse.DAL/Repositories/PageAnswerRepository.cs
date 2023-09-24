using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Repositories
{
    public class PageAnswerRepository: IBaseRepository<PageAnswer>
    {
        private readonly ApplicationDbContext _db;

        public PageAnswerRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(PageAnswer entity)
        {
            await _db.PageAnswers.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(PageAnswer entity)
        {
            _db.PageAnswers.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<PageAnswer> Update(PageAnswer entity)
        {
            _db.PageAnswers.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<PageAnswer> GetAll()
        {
            return _db.PageAnswers;
        }
    }
}
