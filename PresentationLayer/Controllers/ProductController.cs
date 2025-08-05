using BusinessLayer.DTOs.Products;
using BusinessLayer.Managers.Products;
using BusinessLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionRequests.Products;
using PresentationLayer.Mappings;
using PresentationLayer.VMs.Products;

namespace PresentationLayer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager; //= new ProductManager(); // Composition ===> Tightly Coupled
        private readonly FileService _fileService = new FileService();

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }
        // 🚀 /product/index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1- Catch Request

            // 2- Call Model    ---->  Model returns data
            var productDTOs = await _productManager.GetAll();

            var productVMS = productDTOs
                .Select(dto => new GetAllProductsVM
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    Image = dto.Image,
                })
                .ToList();

            // 3- send data to view
            //return View();
            //return View("Index");
            //return View("ProductList", productVMS);
            return View("ProductList2", productVMS);
        }
        [HttpGet]

        //          /product/details/:id      ✅ Path Variable
        //          /product/details?id=3     ✅ Query Parameter
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productManager.GetById(id);

            if(product == null)
            {
                return NotFound();
            }

            var productVM = new GetProductByIdVM
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Description = product.Description,
            };

            return View("Details", productVM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductActionRequest request)
        {
            if(ModelState.IsValid)
            {
                // 1- Upload Image
                var uniqueFileName = _fileService.UploadFile(request.Image, "Images");

                // 2- new product object
                var productDto = new CreateProductDto
                {
                    //Id = _productManager.GetMaxId() + 1,
                    Name = request.Name,
                    Price = request.Price,
                    Description = request.Description,
                    Image = uniqueFileName
                };

                // 3- add to the products list
                await _productManager.CreateProduct(productDto);

                // 4- Redirection

                //return RedirectToAction("Index");
                return RedirectToAction(nameof(Index));
            }

            return View(request);
            
        }

        [HttpGet]
        //   /product/update/:id
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productManager.GetById(id);

            var updateProductActionRequest = new UpdateProductActionRequest
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
            };

            return View(updateProductActionRequest);
        }

        //[HttpPost]
        ////   /product/update
        //public IActionResult Update(UpdateProductActionRequest request)
        //{
        //    // var id = Request.RouteValues[2]       /Product/Update/1  ===> ["product","update","1"]
        //    //Request.Body
        //    //Request.Query

        //    var productDto = request.ToDto();

        //    _productManager.UpdateProduct(productDto);

        //    return RedirectToAction(nameof(Details), new { id = request.Id });
        //}

        [HttpPost]
        //   /product/update/1
        public IActionResult Update(int id, UpdateProductActionRequest2 request)
        {
            //// var id = Request.RouteValues[2]       /Product/Update/1  ===> ["product","update","1"]
            ////Request.Body
            ////Request.Query

            var productDto = new UpdateProductDto
            {
                Id = id,
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
            };

            _productManager.UpdateProduct(productDto);

            return RedirectToAction(nameof(Details), new { id = id });
        }
        public async Task<IActionResult> CheckName(string name)
        {
            var product = await _productManager.GetProductByName(name);

            if(product == null)
            {
                return Json(true);
            }

            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductPartial(int id)
        {
            var product = await _productManager.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            var productVM = new GetProductByIdVM
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Description = product.Description,
            };

            return PartialView("_ProductCardPartial", productVM);
        }

        [HttpPost]
        public async Task<IActionResult> GetProductPartial([FromBody] GetProductByIdVM productVM)
        {
            if(productVM == null)
            {
                return BadRequest("Product data is missing");
            }

            return PartialView("_ProductCardPartial", productVM);
        }
    }

}
