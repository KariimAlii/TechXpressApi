using DataAccessLayer.Repositories.Products;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Validators
{
    public class UniqueNameValidatorAttribute : ValidationAttribute
    {

        // No Dependency Injection in Attribute

        //private readonly IProductRepository productRepository;

        //public UniqueNameValidatorAttribute(IProductRepository productRepository)
        //{
        //    this.productRepository = productRepository;
        //}


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var serviceProvider = validationContext.GetService<IServiceProvider>();

            var productRepo = serviceProvider.GetService<IProductRepository>();

            var product = productRepo.GetByName(value.ToString());

            if(product != null) // if product exists
            {
                return new ValidationResult("Product Name already exists");
            }

            return ValidationResult.Success;
        }
    }
}
