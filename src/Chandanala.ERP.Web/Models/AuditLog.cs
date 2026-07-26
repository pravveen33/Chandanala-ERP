using System.ComponentModel.DataAnnotations;

namespace Chandanala.ERP.Web.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Module { get; set; } = string.Empty; // e.g. "Medicine Master", "Purchase", "Billing"

        [Required]
        [StringLength(100)]
        public string EntityName { get; set; } = string.Empty;

        public int EntityId { get; set; }

        [Required]
        [StringLength(20)]
        public string Action { get; set; } = string.Empty; // "Create", "Update", "Delete"

        [StringLength(500)]
        public string? Details { get; set; }

        // Placeholder until user/auth context is wired in — replace with the
        // logged-in user's identity once authentication is added.
        [StringLength(100)]
        public string PerformedBy { get; set; } = "System";

        public DateTime PerformedOn { get; set; } = DateTime.UtcNow;

        // Deferred to v2.0, once authentication and multi-user auditing are in place:
        // OldValues / NewValues (JSON diff of the change), IpAddress, Success, ErrorMessage.
    }

    /// <summary>
    /// Canonical values for AuditLog.Action, kept as constants so call sites can't
    /// drift into inconsistent casing ("create" vs "Create" vs "Created").
    /// </summary>
    public static class AuditActions
    {
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Cancel = "Cancel";
        public const string Delete = "Delete";
    }
}
