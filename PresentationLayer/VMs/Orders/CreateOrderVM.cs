using BusinessLayer.DTOs.Customers;
using BusinessLayer.DTOs.Products;

namespace PresentationLayer.VMs.Orders
{
    public class CreateOrderVM
    {
        public List<GetAllCustomersDto> Customers { get; set; }
        public List<GetAllProductsDto> Products { get; set; }
    }
}
