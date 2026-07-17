using Chandanala.ERP.Web.AI.Models;

namespace Chandanala.ERP.Web.AI.Abstractions;

public interface IAiProvider
{
    string Name { get; }

    Task<AiResponse> GenerateAsync(
        AiRequest request,
        CancellationToken cancellationToken = default);
}