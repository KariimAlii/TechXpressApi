using BusinessLayer.DTOs.Products;
using BusinessLayer.Managers.Products;
using BusinessLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PresentationLayer.Api.ActionRequests;
using PresentationLayer.Api.Filters;
using PresentationLayer.Api.Models;
using PresentationLayer.VMs.Products;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace PresentationLayer.Api.Controllers
{


    //[Route("api/[controller]")]    //    /api/product
    // [Route("api/[controller]/[action]")]    //    /api/product/.....
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly FileService _fileService = new FileService();

        private readonly IProductManager _productManager;
        private readonly ILogger<ProductController> _logger;
        private readonly IDistributedCache _cache;

        public ProductController(IProductManager productManager, ILogger<ProductController> logger, IDistributedCache cache)
        {
            _productManager = productManager;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        //[Authorize]
        //[ServiceFilter(typeof(LogExecutionTimeFilter))]
        //[ServiceFilter(typeof(LogExecutionTimeFilterAsync))]
        //[ServiceFilter(typeof(IPWhiteListAuthorizationFilter))]
        [ServiceFilter(typeof(RedisCacheResourceFilter))]
        public async Task<ActionResult<List<GetAllProductsResponse>>> Index()
        {
            // 1- Catch Request

            // 2- Call Model    ---->  Model returns data
            //_logger.LogInformation("Getting List of Products");

            //throw new NullReferenceException("Error during fetching products");

            var productDTOs = await _productManager.GetAll();

            var productVMS = productDTOs
                .Select(dto => new GetAllProductsResponse
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price,
                    Image = dto.Image,
                })
                .ToList();

            return Ok(productVMS);
        }

        [HttpGet("id")]                //api/products/id
        //[HttpGet("{id}")]    // /api/products/:id
        //[HttpGet("{id}")]    // /api/products/:id
        public async Task<ActionResult<GetProductByIdResponse>> Details(int id)
        {
            var cacheKey = $"Product_{id}";

            string serializedProduct;

            // Find product by its key in cache

            serializedProduct = await _cache.GetStringAsync(cacheKey);

            // check if the product exists in cache ==> return Ok response

            if(!string.IsNullOrEmpty(serializedProduct))
            {
                // product exists in cache
                var cachedProduct = JsonSerializer.Deserialize<GetProductByIdDto>(serializedProduct);
                var cachedProductVM = new GetProductByIdResponse
                {
                    Id = cachedProduct.Id,
                    Name = cachedProduct.Name,
                    Price = cachedProduct.Price,
                    Image = cachedProduct.Image,
                    Description = cachedProduct.Description,
                };
                return Ok(cachedProductVM);
            }

            // if the product doesn't exist in cache ==> fetch product from database

            var product = await _productManager.GetById(id);  // database

            // serialization
            serializedProduct = JsonSerializer.Serialize(product);

            // set product in cache
            await _cache.SetStringAsync(cacheKey, serializedProduct, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            });

            if (product == null)
            {
                return NotFound();
            }

            var productVM = new GetProductByIdResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Description = product.Description,
            };

            return Ok(productVM);
        }

        //[HttpGet("{id2:int}")]
        //public async Task<ActionResult<GetProductByIdResponse>> Details2(int id2)
        //{
        //    return Ok(id2);
        //}

        //[HttpGet("{id:guid}")]
        //public async Task<ActionResult<GetProductByIdResponse>> Details(Guid id)
        //{
        //    return Ok(id);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductActionRequest request)
        {
            
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            // 1- Upload Image
            var uniqueFileName = _fileService.UploadFile_Api(request.Image, "Images");

            // how to access current user id
            //var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

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
            //return Created();
            //string url = $"https://localhost:7112/api/products/{1}";
            //return Created(url, productDto);
            return CreatedAtAction(nameof(Details), new { id = Guid.NewGuid().ToString() }, productDto);

        }

        //   /api/products/{id}
        [HttpPut("{id}")]
        //[HttpPut]
        //[Route("{id}")]
        public IActionResult UpdateProduct(int id, [FromForm] UpdateProductActionRequest product)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateProduct2([FromQuery] int id, [FromHeader] string name, [FromBody] UpdateProductActionRequest2 product)
        {
            return Ok();
        }

        //     /api/products/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if(id == 2)   // suppose product with id (2) exists in database
            {
                // perform delete
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet("category/{category?}")]
        public IActionResult GetProductByCategory(string? category)
        {
            return Ok();
        }

        #region Model Binder
        // Rule
        //======================
        // ✅ URL ==> Primitive Types Parameters ( int , double , string , bool , ...)
        // ✅ Request Body ==> Complex Types Parameters ( Product , Student , Department , ...)


        //➡️➡️/api/products/students

        //{
        //    id:1,
        //    name:"Mohamed",
        //    isSucceed:true
        //}

        [HttpPost("students")]
        public IActionResult CreateStudent(Student student)
        {
            return Ok();
        }

        [HttpPost("students-2")]
        public IActionResult CreateStudent2(int Id, string Name, bool IsSucceed)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSucceed { get; set; }
    }
}
