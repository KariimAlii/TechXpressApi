using DataAccessLayer.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.VMs.Departments;
using PresentationLayer.VMs.Products;

namespace PresentationLayer.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await _dbContext.Departments
                // Projection ( Select some columns to bring from DB)
                .Select(dep => new DepartmentVM
                {
                    Id = dep.Id,
                    Name = dep.Name
                })
                .ToListAsync();

            return View(departments);
        }
        [HttpGet]
        public async Task<IActionResult> Products(int deptId)
        {


            // ❌❌❌❌❌❌
            //// from DB
            //var products = await _dbContext.Products
            //    .Where(product => product.DepartmentId == deptId)
            //    .ToListAsync();         // Select * from Products  WHERE departmentId = ✅


            //// in Memory
            //var productVMs = products
            //    .Select(product => new ProductListVM
            //    {
            //        Id = product.Id,
            //        Name = product.Name
            //    })
            //    .ToList();


            // ✅✅✅✅✅✅✅✅✅
            // from DB ( query optimization )
            var productss = await _dbContext.Products
               .Where(product => product.DepartmentId == deptId)
               .Select(product => new ProductListVM
               {
                   Id = product.Id,
                   Name = product.Name
               })                        // Select Id , Name from Products  WHERE departmentId = ✅
               .ToListAsync();


            return Json(productss);

        }
    }
}
