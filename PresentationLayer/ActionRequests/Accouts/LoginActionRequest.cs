using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ActionRequests.Accouts
{
    public class LoginActionRequest
    {
        [Required]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
