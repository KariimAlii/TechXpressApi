using BusinessLayer.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers.Orders
{
    public interface IOrderManager
    {
        Task CreateOrder(CreateOrderDto createOrderDto);
        Task<OrderDetailsDto> GetOrderById(int id);
        Task UpdateOrder(UpdateOrderDto updateOrderDto);
    }
}
