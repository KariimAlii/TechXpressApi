using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Orders
{
    public class CreateOrderDto
    {
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public Dictionary<int, int> ProductAmounts { get; set; }
    }

    public static class CreateOrderDtoExtensions
    {
        public static Order ToOrder(this CreateOrderDto dto)
        {
            return new Order
            {
                Rating = default,
                Review = default,
                OrderProducts = new List<OrderProduct>(),
                Date = DateTime.Now,
                CustomerId = dto.CustomerId,
            };
        }
    }
}
