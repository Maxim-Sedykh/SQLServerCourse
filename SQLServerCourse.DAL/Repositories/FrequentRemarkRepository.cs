using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Repositories
{
    public class FrequentRemarkRepository: IBaseRepository<FrequentRemark>
    {
        private readonly ApplicationDbContext _db;

        public FrequentRemarkRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(FrequentRemark entity)
        {
            await _db.FrequentRemarks.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(FrequentRemark entity)
        {
            _db.FrequentRemarks.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<FrequentRemark> Update(FrequentRemark entity)
        {
            _db.FrequentRemarks.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<FrequentRemark> GetAll()
        {
            return _db.FrequentRemarks;
        }
    }
}
