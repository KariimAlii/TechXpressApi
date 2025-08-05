using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> Create(T entity)
        {
            // _dbContext.Products.AddAsync(entity);
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }
        public T Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return entity;
        }
        public void Delete(int id)
        {
            T entity = _dbContext.Set<T>().Find(id);
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWith(string[]? includes = null, Expression<Func<T, bool>>? filter = null)
        {
            // 🚩🚩 IQueryable vs IEnumerable

            // _dbContext.Products                    ===> IQueryable<Product>       ===>  No Journey to Database ❌❌


            // _dbContext.Products.ToListAync()       ===> IEnumerable<Product>      ===>  Journey to Database ✅✅

            // _dbContext.Products.AsEnumerable       ===> IEnumerable<Product>


            //var products = _dbContext.Products
            //    .Where(p => p.Price > 50000)         // where Products.Price > ✅
            //    .Include(p => p.OrderProducts)       // LEFT JOIN OrderProducts
            //    .Select(p => new                     // Projection Select P.Id
            //    {
            //        Id = p.Id
            //    });          //  ===> IQueryable  ===> No Journey to Database

            //var products2 = await _dbContext.Products
            //    .Where(p => p.Price > 50000)
            //    .Include(p => p.OrderProducts)
            //    .Select(p => new
            //    {
            //        Id = p.Id
            //    })
            //    .ToListAsync(); //  ===> IEnumerable ===> Journey to Database

            IQueryable<T> query = _dbContext.Set<T>();

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(includes != null)
            {
                foreach (string include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }


        public async Task<int> Count()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task<int> CountWhere(Expression<Func<T, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T?> GetByIdWith(int id, string? include = null, Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.Include(include).FirstOrDefaultAsync();
        }
    }
}
