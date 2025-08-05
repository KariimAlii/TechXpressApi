using BusinessLayer.Managers.Customers;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerManager _customerManager;

        public CustomerController(ICustomerManager customerManager)
        {
            _customerManager = customerManager;
        }
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerManager.GetCustomerDetailsWithOrders(id);
            return Json(customer);
        }
    }
}
