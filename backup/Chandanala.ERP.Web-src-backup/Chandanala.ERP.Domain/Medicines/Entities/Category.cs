using Chandanala.ERP.Domain.Common.Base;

namespace Chandanala.ERP.Domain.Medicines.Entities;

public sealed class Category : AuditableEntity
{
    private Category()
    {
    }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public static Category Create(
        string code,
        string name)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Category code is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.");

        return new Category
        {
            Code = code.Trim(),
            Name = name.Trim(),
            IsActive = true
        };
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(nameof(name));

        Name = name.Trim();
    }

    public void ChangeDescription(string? description)
    {
        Description = description?.Trim();
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}
