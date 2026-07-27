using Chandanala.ERP.Web.Models;

namespace Chandanala.ERP.Modules.Identity.Domain.Entities;

public class Role
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    private readonly List<Permission> _permissions = new();

    public IReadOnlyCollection<Permission> Permissions => _permissions;

    private Role()
    {
    }

    public Role(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}