using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Api.ActionRequests
{
    public class CreateRoleActionRequest
    {
        [Required]
        public string RoleName { get; set; }
    }
}
