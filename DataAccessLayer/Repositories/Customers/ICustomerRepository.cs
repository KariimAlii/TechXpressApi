using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Customers
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll();
        Customer? GetById(int id);
        void CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void Delete(int id);
    }
}
