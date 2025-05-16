using Microsoft.AspNetCore.Identity;

namespace ECommerceCore.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string EntityName { get; set; }  // e.g., "Product", "Order"
        public string Action { get; set; }      // e.g., "Read", "Write"
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
    public class RolePermission
    {
        public string RoleId { get; set; }
        public IdentityRole? Role { get; set; }
        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}