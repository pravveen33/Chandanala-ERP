using Chandanala.ERP.Application.Interfaces.Security;
using Chandanala.ERP.Application.Invoices.Commands;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Chandanala.ERP.Application.Security.Validators
{
    public class AiInvoiceProposalVgpValidator : IVgpValidator
    {
        // GSTIN Format: 2 digits, 10 chars (PAN), 1 digit, 1 char, 1 char/digit
        private static readonly Regex GstinRegex = new Regex(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", RegexOptions.Compiled);

        public Task<VgpValidationResult> ValidateAsync(object proposalData, CancellationToken cancellationToken)
        {
            if (proposalData is not SubmitAiInvoiceProposalCommand command)
            {
                return Task.FromResult(VgpValidationResult.Failure("Invalid proposal payload type. Expected Invoice Proposal."));
            }

            var errors = new List<string>();

            // 1. Hallucination / Boundary Check: Impossible financial states
            if (command.ProposedTotal <= 0)
            {
                errors.Add("AI proposed an invalid financial state: Total must be strictly greater than zero.");
            }

            if (command.ProposedTotal > 5_000_000) // Sanity boundary for automated proposals
            {
                errors.Add("AI proposed a total exceeding the autonomous transaction limit.");
            }

            // 2. Syntax / Format Check: Did the model hallucinate a fake GSTIN format?
            if (string.IsNullOrWhiteSpace(command.VendorGstin) || !GstinRegex.IsMatch(command.VendorGstin))
            {
                errors.Add("AI proposed a malformed Vendor GSTIN.");
            }

            // 3. Payload Integrity: Did the OCR actually return data, or did the model skip it?
            if (string.IsNullOrWhiteSpace(command.RawOcrText) || command.RawOcrText.Length < 10)
            {
                errors.Add("AI proposal lacks sufficient source evidence (OCR Text) for audit logging.");
            }

            return errors.Count > 0 
                ? Task.FromResult(VgpValidationResult.Failure(errors)) 
                : Task.FromResult(VgpValidationResult.Success());
        }
    }
}
