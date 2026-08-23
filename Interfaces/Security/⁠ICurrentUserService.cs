using System;

namespace Chandanala.ERP.Application.Interfaces.Security
{
    public interface ICurrentUserService
    {
        Guid? TenantId { get; }
        Guid? UserId { get; }
    }
}
