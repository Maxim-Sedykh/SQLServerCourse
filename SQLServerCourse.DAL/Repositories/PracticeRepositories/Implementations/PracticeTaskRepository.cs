using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.DAL.Repositories.PracticeRepositories.Implementations
{
    public class PracticeTaskRepository
    {
        private readonly ApplicationDbContext _db;

        public PracticeTaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> SelectFilm(FrequentRemark entity)
        {
            await _db.FrequentRemarks.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SelectHall(FrequentRemark entity)
        {
            await _db.FrequentRemarks.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SelectHallRow(FrequentRemark entity)
        {
            await _db.FrequentRemarks.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SelectScreening(FrequentRemark entity)
        {
            await _db.FrequentRemarks.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SelectTickets(FrequentRemark entity)
        {
            await _db.FrequentRemarks.AddAsync(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        
    }
}
