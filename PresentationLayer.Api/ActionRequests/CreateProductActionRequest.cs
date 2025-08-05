using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Api.Validators;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Api.ActionRequests
{
    public class CreateProductActionRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Name cannot exceed 100 characters")]
        //[UniqueNameValidator]
        [Remote(action: "CheckName", controller: "Product", ErrorMessage = "Name is used before")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }

        [EmailAddress]  //  ....@....

        // @google.com , @microsoft.com
        [MyEmailDomain("microsoft.com", "google.com")]
        public string Email { get; set; }
    }
}
