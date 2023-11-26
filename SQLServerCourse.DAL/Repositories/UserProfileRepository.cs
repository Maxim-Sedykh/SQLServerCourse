using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Repositories
{
    public class UserProfileRepository: IBaseRepository<UserProfile>
    {
        private readonly ApplicationDbContext _db;

        public UserProfileRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(UserProfile entity)
        {
            await _db.UserProfiles.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(UserProfile entity)
        {
            _db.UserProfiles.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<UserProfile> Update(UserProfile entity)
        {
            _db.UserProfiles.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public IQueryable<UserProfile> GetAll()
        {
            return _db.UserProfiles;
        }
    }
}
