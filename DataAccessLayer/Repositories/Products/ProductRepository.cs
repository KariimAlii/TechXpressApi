using DataAccessLayer.Common;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetAll()
        {
           var products =  await _dbContext.Products.AsNoTracking().ToListAsync();

            return products;
        }
        public async Task<Product?> GetById(int id)
        {
            // LINQ
            return await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(product => product.Id == id);
        }
        public async Task CreateProduct(Product product)
        {
            //if (product != null)
            //{
            //    _dbContext.Products.Add(product);
            //}

            if (product is null)
                throw new ArgumentNullException("Product shouldnot be null");

            await _dbContext.Products.AddAsync(product);

            //await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateProduct(Product product)
        {
            var currentProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            if (currentProduct is not null)
            {
                currentProduct.Price = product.Price;
                currentProduct.Name = product.Name;
                currentProduct.Description = product.Description;
                //currentProduct.Image = product.Image;
            }
            //await _dbContext.SaveChangesAsync();

            //_dbContext.Products.Update(product);
        }
        public async Task Delete(int id)
        {
            var currentProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            //var currentProduct = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id); // ==> entry state is detached ==> the entry is not marked as deleted ==> if you save changes ==> the entity will not be deleted from database
            if (currentProduct is not null)
            {
                _dbContext.Products.Remove(currentProduct);
            }
        }
        public async Task<int> GetMaxId()
        {
            int maxId = 0;

            if(_dbContext.Products.Count() > 0)
            {
                maxId = await _dbContext.Products.MaxAsync(p => p.Id);
            }

            return maxId;
        }

        public async Task<Product?> GetByName(string name)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(product => product.Name == name);
        }

        public async Task<PagedList<Product>> GetProductsAsync(ProductQueryParameters parameters)
        {
            // Build Query Step by Step

            var query = _dbContext.Products.AsQueryable();

            if(!string.IsNullOrWhiteSpace(parameters.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(parameters.Name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(parameters.DepartmentName))
            {
                query = query.Where(p => p.Department.Name.ToLower().Contains(parameters.DepartmentName.ToLower()));
            }

            var totalCount = await query.CountAsync();

            if(parameters.SortBy.ToLower() == "name")
            {
                query = parameters.SortDesc
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name);
            }

            if (parameters.SortBy.ToLower() == "price")
            {
                query = parameters.SortDesc
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price);
            }

            // Pagination ( Optional ) only if the user sent page no & page size

            if
            (
                parameters.PageNo.HasValue && parameters.PageNo.Value > 0 &&
                parameters.PageSize.HasValue && parameters.PageSize.Value > 0 
            )
            {
                var pageNo = parameters.PageNo.Value;
                var pageSize = parameters.PageSize.Value;

                query = query
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);

                // if pageNo = 3 , pageSize = 11
                //         ==> skip 22 record , take next 11 record

                
            }

            var products = await query.ToListAsync();

            return new PagedList<Product>
            {
                TotalCount = totalCount,
                Data = products,
            };
        }
    }
}
