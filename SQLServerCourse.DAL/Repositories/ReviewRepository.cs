using Microsoft.EntityFrameworkCore;
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
    public class ReviewRepository : IBaseRepository<Review>
    {
        private readonly CourseDbContext _db;

        public ReviewRepository(CourseDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Review entity)
        {
            await _db.Reviews.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }   

        public async Task<bool> Delete(Review entity)
        {
            _db.Reviews.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Review> Update(Review entity)
        {
            _db.Reviews.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<Review> GetAll()
        {
            return _db.Reviews;
        }
    }
}
