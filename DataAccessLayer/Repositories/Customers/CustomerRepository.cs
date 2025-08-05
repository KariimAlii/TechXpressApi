using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CreateCustomer(Customer customer)
        {
            if (customer is null)
                throw new ArgumentNullException("Customer shouldnot be null");

            _dbContext.Customers.Add(customer);

            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var currentCustomer = _dbContext.Customers.FirstOrDefault(c => c.Id == id);

            if (currentCustomer is not null)
            {
                _dbContext.Customers.Remove(currentCustomer);
            }
        }

        public List<Customer> GetAll()
        {
            return _dbContext.Customers.ToList();
        }

        public Customer? GetById(int id)
        {
            return _dbContext.Customers.FirstOrDefault(customer => customer.Id == id);
        }

        public void UpdateCustomer(Customer customer)
        {
            var currentCustomer = _dbContext.Customers.FirstOrDefault(c => c.Id == customer.Id);
            if (currentCustomer is not null)
            {
                currentCustomer.Name = customer.Name;
                currentCustomer.Address = customer.Address;
                currentCustomer.PhoneNumber = customer.PhoneNumber;

            }

            _dbContext.SaveChanges();
        }
    }
}
