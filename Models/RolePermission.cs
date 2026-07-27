namespace Chandanala.ERP.Web.Models;

public class RolePermission
{
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long PermissionId { get; set; }

    public bool IsActive { get; set; } = true;
}