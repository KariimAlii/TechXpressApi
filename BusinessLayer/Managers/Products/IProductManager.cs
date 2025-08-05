using BusinessLayer.DTOs.Products;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers.Products
{
    public interface IProductManager
    {
        Task<List<GetAllProductsDto>> GetAll();
        Task<GetProductByIdDto?> GetById(int id);
        Task CreateProduct(CreateProductDto dto);
        Task UpdateProduct(UpdateProductDto dto);
        Task<Product?> GetProductByName(string name);
    }
}
