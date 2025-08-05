using BusinessLayer.DTOs.Products;
using PresentationLayer.ActionRequests.Products;

namespace PresentationLayer.Mappings
{
    public static class ProductMappings
    {
        public static UpdateProductDto ToDto(this UpdateProductActionRequest request)
        {
            return new UpdateProductDto
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
            };
        }
    }
}
