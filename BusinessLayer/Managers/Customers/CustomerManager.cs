using BusinessLayer.DTOs.Customers;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers.Customers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly IGenericRepository<Customer> _customerRepository;

        public CustomerManager(IGenericRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<CustomerDetailsDto> GetCustomerDetailsWithOrders(int CustomerId)
        {
            //var customer = await _customerRepository.GetByIdWith(CustomerId, "Orders", c => c.Address.StartsWith("Alex"));
            var customer = await _customerRepository.GetByIdWith(CustomerId, "Orders", null);

            if (customer is not null)
            {
                var dto = new CustomerDetailsDto
                {
                    Name = customer.Name,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    Orders = customer.Orders.Select(o => new CustomerOrderDto
                    {
                        Id = o.Id,
                        Date = o.Date,
                        Rating = o.Rating,
                        Review = o.Review
                    }).ToList()
                };

                return dto;
            }
            return null;
        }

        public async Task<List<GetAllCustomersDto>> GetAllCustomers()
        {
            var customersFromDB = await _customerRepository.GetAll();

            var customers = customersFromDB
                .Select(customer => (GetAllCustomersDto) customer)
                .ToList();

            return customers;
        }
    }
}
