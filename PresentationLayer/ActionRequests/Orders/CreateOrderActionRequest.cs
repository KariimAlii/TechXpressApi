using BusinessLayer.DTOs.Orders;

namespace PresentationLayer.ActionRequests.Orders
{
    public class CreateOrderActionRequest
    {
        public int CustomerId { get; set; }
        public Dictionary<int, int> ProductAmounts { get; set; }
        public CreateOrderDto ToDto()
        {
            return new CreateOrderDto
            {
                CustomerId = CustomerId,
                ProductAmounts = ProductAmounts
            };
        }
    }
}
