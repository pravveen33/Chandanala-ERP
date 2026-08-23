using Chandanala.ERP.Application.Invoices.Commands;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Chandanala.ERP.Application.Interfaces.Infrastructure
{
    /// <summary>
    /// Acts as the boundary for untrusted multimodal observation.
    /// Parses external documents into structured AI proposals.
    /// </summary>
    public interface IOcrInvoiceParser
    {
        Task<SubmitAiInvoiceProposalCommand> ParseInvoiceAsync(
            Stream fileStream, 
            string fileName, 
            CancellationToken cancellationToken);
    }
}
