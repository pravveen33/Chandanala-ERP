using System.ComponentModel.DataAnnotations;

namespace Chandanala.ERP.Web.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ShortName { get; set; }

        [StringLength(50)]
        public string? LicenseNumber { get; set; }

        [StringLength(15)]
        public string? GSTNumber { get; set; }

        [StringLength(200)]
        public string? ContactPerson { get; set; }

        [StringLength(20)]
        public string? Mobile { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(10)]
        public string? Pincode { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}