using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Api.ActionRequests
{
    public class LoginUserActionRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
