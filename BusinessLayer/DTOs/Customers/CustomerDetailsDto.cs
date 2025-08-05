using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs.Customers
{
    public class CustomerDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<CustomerOrderDto> Orders { get; set; }
    }

    public class CustomerOrderDto
    {
        public int Id { get; set; }

        public int? Rating { get; set; }

        public string Review { get; set; }

        public DateTime Date { get; set; }
    }
}
