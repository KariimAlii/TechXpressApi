using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Orders
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public int? Rating { get; set; }
        public string? Review { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
