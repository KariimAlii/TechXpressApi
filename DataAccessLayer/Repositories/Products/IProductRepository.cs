using DataAccessLayer.Common;
using DataAccessLayer.Entities;
using DataAccessLayer.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Products
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task<Product?> GetByName(string name);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task Delete(int id);
        Task<int> GetMaxId();
        Task<PagedList<Product>> GetProductsAsync(ProductQueryParameters parameters);
    }
}
