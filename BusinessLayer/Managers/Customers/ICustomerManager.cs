using BusinessLayer.DTOs.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers.Customers
{
    public interface ICustomerManager
    {
        Task<CustomerDetailsDto> GetCustomerDetailsWithOrders(int CustomerId);
        Task<List<GetAllCustomersDto>> GetAllCustomers();
    }
}
