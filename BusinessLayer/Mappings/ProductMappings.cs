using BusinessLayer.DTOs.Products;
using DataAccessLayer.Entities;

namespace BusinessLayer.Mappings
{
    public static class ProductMappings
    {
        public static Product ToEntity(this UpdateProductDto dto)
            => new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
            };
    }
    
}
