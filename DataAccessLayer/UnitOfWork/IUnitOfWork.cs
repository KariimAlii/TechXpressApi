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
    public interface IUnitOfWork
    {
        IGenericRepository<Order> OrderRepository { get; }
        IProductRepository ProductRepository { get; }
        Task<int> SaveChanges();
    }
}
