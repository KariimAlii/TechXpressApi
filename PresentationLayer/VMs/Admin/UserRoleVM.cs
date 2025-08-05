using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.VMs.Admin
{
    public class UserRoleVM
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
    }
}
