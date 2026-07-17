using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chandanala.ERP.Web.Models
{
    public class Medicine : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? MedicineCode { get; set; }

        [StringLength(50)]
        public string? Barcode { get; set; }

        [Required(ErrorMessage = "Brand Name is required")]
        [StringLength(200)]
        public string BrandName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Generic Name is required")]
        [StringLength(200)]
        public string GenericName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Strength is required")]
        [StringLength(50)]
        public string Strength { get; set; } = string.Empty;

        [StringLength(50)]
        public string? DosageForm { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [StringLength(50)]
        public string? PackType { get; set; }

        [StringLength(50)]
        public string? PackSize { get; set; }

        [StringLength(20)]
        public string? Unit { get; set; }

        [StringLength(200)]
        public string? ManufacturerName { get; set; }

        [StringLength(50)]
        public string? ScheduleType { get; set; }

        [StringLength(20)]
        public string? HsnCode { get; set; }

        [Range(0, 100, ErrorMessage = "GST % must be between 0 and 100")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal GstPercent { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Purchase Price cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MRP cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MRP { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Selling Price cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; }

        [Range(0, int.MaxValue)]
        public int MinimumStock { get; set; }

        [Range(0, int.MaxValue)]
        public int MaximumStock { get; set; }

        [StringLength(20)]
        public string? RackNumber { get; set; }

        [StringLength(20)]
        public string? ShelfNumber { get; set; }

        [StringLength(100)]
        public string? StorageCondition { get; set; }

        [StringLength(1000)]
        public string? Remarks { get; set; }

        public bool PrescriptionRequired { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        // Soft-delete fields — kept separate from IsActive, which is a business
        // "is this medicine currently sellable" flag the user sets deliberately.
        // IsDeleted marks a record removed from the UI while preserving history.
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedOn { get; set; }

        // Custom cross-field validation (MRP should not be less than Purchase Price, etc.)
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (SellingPrice > MRP && MRP > 0)
            {
                results.Add(new ValidationResult(
                    "Selling Price cannot exceed MRP.",
                    new[] { nameof(SellingPrice) }));
            }

            if (MRP < PurchasePrice && MRP > 0)
            {
                results.Add(new ValidationResult(
                    "MRP cannot be less than Purchase Price.",
                    new[] { nameof(MRP) }));
            }

            if (MinimumStock > MaximumStock && MaximumStock > 0)
            {
                results.Add(new ValidationResult(
                    "Minimum Stock cannot exceed Maximum Stock.",
                    new[] { nameof(MinimumStock) }));
            }

            return results;
        }
    }
    
}