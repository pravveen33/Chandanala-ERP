using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Chandanala.ERP.Application.Interfaces.Security
{
    public record VgpValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; init; } = new();

        public static VgpValidationResult Success() => new();
        public static VgpValidationResult Failure(List<string> errors) => new() { Errors = errors };
        public static VgpValidationResult Failure(string error) => new() { Errors = new List<string> { error } };
    }

    /// <summary>
    /// Evaluates an untrusted AI proposal against strict format, syntax, and boundary invariants.
    /// </summary>
    public interface IVgpValidator
    {
        Task<VgpValidationResult> ValidateAsync(object proposalData, CancellationToken cancellationToken);
    }
}
