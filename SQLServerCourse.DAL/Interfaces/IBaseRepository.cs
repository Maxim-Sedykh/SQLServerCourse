using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace SQLServerCourse.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        public Task<bool> Create(T entity);

        public Task<bool> Delete(T entity);

        public Task<T> Update(T entity);

        public IQueryable<T> GetAll();
    }
}
