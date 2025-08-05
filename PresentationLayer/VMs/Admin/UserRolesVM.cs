namespace PresentationLayer.VMs.Admin
{
    public class UserRolesVM
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
