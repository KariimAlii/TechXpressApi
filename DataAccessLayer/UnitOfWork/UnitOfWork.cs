using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Generic;
using DataAccessLayer.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IGenericRepository<Order> OrderRepository { get; }

        public IProductRepository ProductRepository { get; }

        public UnitOfWork
        (
            IGenericRepository<Order> orderRepository,
            IProductRepository productRepository,
            ApplicationDbContext dbContext
        )
        {
            OrderRepository = orderRepository;
            ProductRepository = productRepository;
            _dbContext = dbContext;
        }
        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
