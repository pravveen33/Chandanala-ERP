namespace Chandanala.ERP.Web.Models;

public class UserRole
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long RoleId { get; set; }

    public bool IsActive { get; set; } = true;
}