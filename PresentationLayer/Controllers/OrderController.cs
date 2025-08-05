using BusinessLayer.Managers.Customers;
using BusinessLayer.Managers.Orders;
using BusinessLayer.Managers.Products;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionRequests.Orders;
using PresentationLayer.VMs.Orders;

namespace PresentationLayer.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICustomerManager _customerManager;
        private readonly IProductManager _productManager;
        private readonly IOrderManager _orderManager;

        // Controller  ----data----> View

        // VM

        public OrderController(ICustomerManager customerManager, IProductManager productManager, IOrderManager orderManager)
        {
            _customerManager = customerManager;
            _productManager = productManager;
            _orderManager = orderManager;
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //var myCustomers = new List<string>
            //{
            //    "A",
            //    "B",
            //    "C",
            //};

            //ViewBag.myCustomers = myCustomers;
            //ViewData["myCustomers"] = myCustomers;

            var customers = await _customerManager.GetAllCustomers();
            var products = await _productManager.GetAll();

            var vm = new CreateOrderVM
            {
                Customers = customers,
                Products = products
            };

            return View("Create2", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderActionRequest request)
        {
            await _orderManager.CreateOrder(request.ToDto());
            return RedirectToAction("Index", "Home");
        }

        //   /Order/Update/1

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var order = await _orderManager.GetOrderById(id);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateOrderActionRequest request)
        {
            var updateOrderDto = request.ToDto();
            updateOrderDto.Id = id;
            await _orderManager.UpdateOrder(updateOrderDto);
            return RedirectToAction("Index", "Home");
        }
    }
}
