using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ActionRequests.Roles
{
    public class CreateRoleActionRequest
    {
        [Required]
        public string RoleName { get; set; }
    }
}
