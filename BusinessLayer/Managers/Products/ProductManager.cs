using BusinessLayer.DTOs.Products;
using BusinessLayer.Mappings;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Products;
using DataAccessLayer.UnitOfWork;

namespace BusinessLayer.Managers.Products
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IProductRepository _productRepository; // = new ProductRepository();  ==> creation of object is delegated Dependency Injection Container
        //public ProductManager(IProductRepository productRepository)
        //{
        //    _productRepository = productRepository;
        //}

        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAllProductsDto>> GetAll()
        {
            // Business Logic
            var productEntities = await _unitOfWork.ProductRepository.GetAll();

            //var productDTOs = new List<GetAllProductsDto>();

            //foreach (var product in productEntities)
            //{
            //    productDTOs.Add(new GetAllProductsDto
            //    {
            //        Id = product.Id,
            //        Name = product.Name,
            //        Price = product.Price,
            //        Image = product.Image,
            //    });
            //}

            var productDTOs = productEntities
            .Select(p => new GetAllProductsDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Image = p.Image,
            })
            .ToList();

            return productDTOs;

        }
        public async Task<GetProductByIdDto?> GetById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetById(id);

            if (product == null)
                return null;

            var productDto = new GetProductByIdDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Description = product.Description,
            };

            return productDto;
        }
        public async Task CreateProduct(CreateProductDto dto)
        {
            var product = new Product
            {
                Id = await _unitOfWork.ProductRepository.GetMaxId() + 1,
                Name = dto.Name,
                Price = dto.Price,
                Image = dto.Image,
                Description = dto.Description,
                DepartmentId = 1
            };

            await _unitOfWork.ProductRepository.CreateProduct(product);
            await _unitOfWork.SaveChanges();
        }
        public async Task UpdateProduct(UpdateProductDto dto)
        {
            var product = dto.ToEntity();

            await _unitOfWork.ProductRepository.UpdateProduct(product);
            await _unitOfWork.SaveChanges();
        }

        public async Task<Product?> GetProductByName(string name)
        {
           var product = await _unitOfWork.ProductRepository.GetByName(name);
            return product;
        }
        
    }
}
