using BusinessLayer.DTOs.Orders;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Generic;
using DataAccessLayer.Repositories.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers.Orders
{
    public class OrderManager : IOrderManager
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly ICustomOrderRepo _customOrderRepo;

        public OrderManager(IGenericRepository<Order> orderRepository, ICustomOrderRepo customOrderRepo)
        {
            _orderRepository = orderRepository;
            _customOrderRepo = customOrderRepo;
        }

        public async Task CreateOrder(CreateOrderDto createOrderDto)
        {
            var order = createOrderDto.ToOrder();

            // order.Id = default = 0

            // 1- Insert Order ( Parent )  

            await _orderRepository.Create(order);

            // Order ( Parent )      OrderProduct ( Child )

            // 2- Save Changes
            // Commit ( Save Changes ) ➡️➡️ Execution of Sql Query ( Insert into Orders )
            await _orderRepository.SaveChangesAsync();
            // ef core implicitly ===> order.Id = ✅

            // 3- Insert OrderProducts ( Child )
            order.OrderProducts = createOrderDto.ProductAmounts
                .Select(product => new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = product.Key,
                    Amount = product.Value
                })
                .ToList();

            // 4- Save Changes

            await _orderRepository.SaveChangesAsync();
        }

        public async Task<OrderDetailsDto> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdWith(id, "OrderProducts");

            var orderDto = new OrderDetailsDto
            {
                Id = order.Id,
                Rating = order.Rating,
                Review = order.Review,
                OrderProducts = order.OrderProducts.ToList(),
            };

            return orderDto;
        }

        public async Task UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var order = await _orderRepository.GetById(updateOrderDto.Id);
            var orders = await _orderRepository.GetAll();
            //order.Review = updateOrderDto.Review;
            //order.Rating = updateOrderDto.Rating;

            await _customOrderRepo.UpdateOrderRatingAndReview(order, updateOrderDto.Rating, updateOrderDto.Review);

            await _orderRepository.SaveChangesAsync();
        }
    }
}
