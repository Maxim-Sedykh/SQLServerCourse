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
    public class TestVariantRepository: IBaseRepository<TestVariant>
    {
        private readonly CourseDbContext _db;

        public TestVariantRepository(CourseDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(TestVariant entity)
        {
            await _db.TestVariants.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(TestVariant entity)
        {
            _db.TestVariants.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<TestVariant> Update(TestVariant entity)
        {
            _db.TestVariants.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<TestVariant> GetAll()
        {
            return _db.TestVariants;
        }
    }
}
