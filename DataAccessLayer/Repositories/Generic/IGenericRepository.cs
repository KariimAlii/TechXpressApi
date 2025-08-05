using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T?> GetById(int id);
        Task<T?> GetByIdWith(int id, string? include = null, Expression<Func<T, bool>>? filter = null);
        Task<T> Create(T entity);

        T Update(T entity);
        void Delete(int id);
        Task<int> Count();
        Task<int> CountWhere(Expression<Func<T, bool>>? filter = null);
        Task<IEnumerable<T>> GetWith(string[]? includes = null, Expression<Func<T, bool>>? filter = null);

        Task SaveChangesAsync();

        // _dbContext.Products.Where(product => product.Price > 50000).ToListAsync();
        // _dbContext.Products.CountAsync(product => product.Price > 50000)
    }
}
