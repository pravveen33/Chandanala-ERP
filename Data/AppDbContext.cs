using Microsoft.EntityFrameworkCore;
using Chandanala.ERP.Web.Models;

namespace Chandanala.ERP.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
base.OnModelCreating(modelBuilder);
    {
        configurationBuilder.Properties<DateTime>()
            .HaveColumnType("timestamp with time zone");
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    public DbSet<Medicine> Medicines => Set<Medicine>();

    public DbSet<Supplier> Suppliers => Set<Supplier>();

    public DbSet<SupplierHistory> SupplierHistories => Set<SupplierHistory>();

    public DbSet<SupplierChangeRequest> SupplierChangeRequests => Set<SupplierChangeRequest>();

    public DbSet<Purchase> Purchases => Set<Purchase>();

    public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();

    public DbSet<Batch> Batches => Set<Batch>();

    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();

    public DbSet<Sale> Sales => Set<Sale>();

    public DbSet<SaleItem> SaleItems => Set<SaleItem>();


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplySupplierTimestamps();
        ApplyPurchaseTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplySupplierTimestamps()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<Supplier>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = null;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Property(s => s.CreatedAt).IsModified = false;
            }
        }
    }

    private void ApplyPurchaseTimestamps()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<Purchase>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.UpdatedAt = null;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Property(p => p.CreatedAt).IsModified = false;
            }
        }
    }
    /// <summary>
    /// Every relationship and index in this schema is configured here, via Fluent API,
    /// rather than via navigation properties on the model classes. This is a deliberate
    /// choice, not an oversight: the models currently have no ICollection/reference
    /// navigation properties at all (by design, to keep this change minimal), so every
    /// FK below is a "shape-only" relationship — HasOne/WithMany with an explicit FK
    /// property and no corresponding navigation property on either side. EF Core fully
    /// supports this; it just means you'll query with explicit .Where(x => x.FooId == y)
    /// rather than x.Foo.Bar. If navigation properties are added to the models later,
    /// this configuration can be updated to use WithMany(x => x.Children) etc. without
    /// changing the underlying database schema.
    ///
    /// Delete behavior: master/reference data referenced by transactional history
    /// (Medicine, Supplier, Batch) uses Restrict, so a hard delete is blocked rather
    /// than silently cascading through purchase/stock history. Pure assignment/join
    /// tables (UserRole, RolePermission) use Cascade, since a role assignment has no
    /// independent meaning once its User or Role is gone.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ----- Supplier -----
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(s => s.SupplierCode)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(s => s.SupplierName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(s => s.GSTIN)
                .HasMaxLength(15);

            entity.Property(s => s.DrugLicenseNumber)
                .HasMaxLength(100);

            entity.Property(s => s.ContactPerson)
                .HasMaxLength(200);

            entity.Property(s => s.MobileNumber)
                .HasMaxLength(20);

            entity.Property(s => s.Email)
                .HasMaxLength(100);

            entity.Property(s => s.Address)
                .HasMaxLength(500);

            entity.Property(s => s.City)
                .HasMaxLength(100);

            entity.Property(s => s.State)
                .HasMaxLength(100);

            entity.Property(s => s.Pincode)
                .HasMaxLength(10);

            entity.Property(s => s.CreditLimit)
                .HasPrecision(18, 2);

            entity.Property(s => s.CreatedAt)
                .HasColumnType("timestamp with time zone");

            entity.Property(s => s.UpdatedAt)
                .HasColumnType("timestamp with time zone");

            // Optimistic concurrency token backed by Postgres' native xmin
            // system column — no extra storage, automatically maintained by
            // Postgres on every row version.
            entity.Property(s => s.RowVersion)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        });

        // ----- SupplierHistory -----
        modelBuilder.Entity<SupplierHistory>(entity =>
        {
            entity.Property(h => h.SupplierCode).IsRequired().HasMaxLength(30);
            entity.Property(h => h.SupplierName).IsRequired().HasMaxLength(200);
            entity.Property(h => h.GSTIN).HasMaxLength(15);
            entity.Property(h => h.DrugLicenseNumber).HasMaxLength(100);
            entity.Property(h => h.ContactPerson).HasMaxLength(200);
            entity.Property(h => h.MobileNumber).HasMaxLength(20);
            entity.Property(h => h.Email).HasMaxLength(100);
            entity.Property(h => h.Address).HasMaxLength(500);
            entity.Property(h => h.City).HasMaxLength(100);
            entity.Property(h => h.State).HasMaxLength(100);
            entity.Property(h => h.Pincode).HasMaxLength(10);
            entity.Property(h => h.ChangedBy).IsRequired().HasMaxLength(100);
            entity.Property(h => h.Reason).HasMaxLength(500);

            entity.Property(h => h.CreditLimit).HasPrecision(18, 2);

            entity.Property(h => h.ChangedAt)
                .HasColumnType("timestamp with time zone");

            // Restrict, not Cascade: a supplier is soft-deleted (IsDeleted flag),
            // never hard-deleted, so this should never fire in practice — but if
            // it ever did, silently cascading away audit history would be wrong.
            entity.HasOne(h => h.Supplier)
                .WithMany()
                .HasForeignKey(h => h.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // One history row per (SupplierId, VersionNumber) — guards against
            // ever double-writing the same version if a save is retried.
            entity.HasIndex(h => new { h.SupplierId, h.VersionNumber })
                .IsUnique();
        });

        // ----- SupplierChangeRequest -----
        modelBuilder.Entity<SupplierChangeRequest>(entity =>
        {
            entity.Property(r => r.SupplierCode).IsRequired().HasMaxLength(30);
            entity.Property(r => r.SupplierName).IsRequired().HasMaxLength(200);
            entity.Property(r => r.GSTIN).HasMaxLength(15);
            entity.Property(r => r.DrugLicenseNumber).HasMaxLength(100);
            entity.Property(r => r.ContactPerson).HasMaxLength(200);
            entity.Property(r => r.MobileNumber).HasMaxLength(20);
            entity.Property(r => r.Email).HasMaxLength(100);
            entity.Property(r => r.Address).HasMaxLength(500);
            entity.Property(r => r.City).HasMaxLength(100);
            entity.Property(r => r.State).HasMaxLength(100);
            entity.Property(r => r.Pincode).HasMaxLength(10);
            entity.Property(r => r.ChangedRegulatedFields).IsRequired().HasMaxLength(200);
            entity.Property(r => r.Status).IsRequired().HasMaxLength(20);
            entity.Property(r => r.RequestedBy).IsRequired().HasMaxLength(100);
            entity.Property(r => r.RequestReason).HasMaxLength(500);
            entity.Property(r => r.ReviewedBy).HasMaxLength(100);
            entity.Property(r => r.ReviewNotes).HasMaxLength(500);

            entity.Property(r => r.CreditLimit).HasPrecision(18, 2);

            entity.Property(r => r.RequestedAt)
                .HasColumnType("timestamp with time zone");

            entity.Property(r => r.ReviewedAt)
                .HasColumnType("timestamp with time zone");

            // Restrict, matching SupplierHistory — a supplier is soft-deleted,
            // never hard-deleted, so this is a safety net rather than a real path.
            entity.HasOne(r => r.Supplier)
                .WithMany()
                .HasForeignKey(r => r.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Supports the review-queue query ("show me all Pending requests")
            // without a full table scan as this grows.
            entity.HasIndex(r => r.Status)
                .HasDatabaseName("IX_SupplierChangeRequests_Status");
        });

        // ----- UserRole -----
        modelBuilder.Entity<UserRole>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // ----- RolePermission -----
        modelBuilder.Entity<RolePermission>()
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolePermission>()
            .HasOne<Permission>()
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        // ----- Purchase / PurchaseItem -----
        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.Property(p => p.PurchaseNumber)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(p => p.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Subtotal)
                .HasPrecision(18, 2);

            entity.Property(p => p.DiscountAmount)
                .HasPrecision(18, 2);

            entity.Property(p => p.GSTAmount)
                .HasPrecision(18, 2);

            entity.Property(p => p.OtherCharges)
                .HasPrecision(18, 2);

            entity.Property(p => p.RoundOff)
                .HasPrecision(18, 2);

            entity.Property(p => p.GrandTotal)
                .HasPrecision(18, 2);

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(p => p.Remarks)
                .HasMaxLength(500);

            entity.Property(p => p.CreatedAt)
                .HasColumnType("timestamp with time zone");

            entity.Property(p => p.UpdatedAt)
                .HasColumnType("timestamp with time zone");
        });

        modelBuilder.Entity<Purchase>()
            .HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseItem>()
            .HasOne<Purchase>()
            .WithMany()
            .HasForeignKey(pi => pi.PurchaseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PurchaseItem>(entity =>
        {
            entity.Property(pi => pi.BatchNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(pi => pi.PurchaseRate)
                .HasPrecision(18, 2);

            entity.Property(pi => pi.MRP)
                .HasPrecision(18, 2);

            entity.Property(pi => pi.SellingPrice)
                .HasPrecision(18, 2);

            entity.Property(pi => pi.GSTPercentage)
                .HasPrecision(5, 2);

            entity.Property(pi => pi.DiscountPercentage)
                .HasPrecision(5, 2);

            entity.Property(pi => pi.LineTotal)
                .HasPrecision(18, 2);
        });

        modelBuilder.Entity<PurchaseItem>()
            .HasOne<Medicine>()
            .WithMany()
            .HasForeignKey(pi => pi.MedicineId)
            .OnDelete(DeleteBehavior.Restrict);

        // ----- Batch / StockTransaction -----
        modelBuilder.Entity<Batch>()
            .HasOne<Medicine>()
            .WithMany()
            .HasForeignKey(b => b.MedicineId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransaction>()
            .HasOne<Medicine>()
            .WithMany()
            .HasForeignKey(st => st.MedicineId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransaction>()
            .HasOne<Batch>()
            .WithMany()
            .HasForeignKey(st => st.BatchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockTransaction>()
            .Property(st => st.ReferenceNumber)
            .HasMaxLength(30);

        // ----- Sale / SaleItem -----
        modelBuilder.Entity<SaleItem>()
            .HasOne<Sale>()
            .WithMany()
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade); // a Sale is never hard-deleted (see Sale.cs remarks), so this only matters in edge-case cleanup

        modelBuilder.Entity<SaleItem>()
            .HasOne<Medicine>()
            .WithMany()
            .HasForeignKey(si => si.MedicineId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SaleItem>()
            .HasOne<Batch>()
            .WithMany()
            .HasForeignKey(si => si.BatchId)
            .OnDelete(DeleteBehavior.Restrict);

        // =====================================================================
        // Indexes
        // =====================================================================

        // --- Uniqueness (filtered to exclude soft-deleted rows, so a deleted
        //     record's name/code can be reused by a genuinely new record) ---
        //
        // NOTE — case sensitivity: these are standard B-tree unique indexes, which
        // are case-SENSITIVE in PostgreSQL by default. The existing application-level
        // duplicate checks (Medicines.razor / Manufacturers.razor) use .ToLower()
        // comparisons specifically to catch "Dolo" vs "DOLO" as duplicates — this
        // index does NOT replace that check, it only closes the race-condition gap
        // where two near-simultaneous saves could both pass the app-level check
        // before either commits. If full case-insensitive enforcement at the
        // database level is wanted later, that needs either a citext column type
        // or a computed lowercase shadow property with its own unique index —
        // deliberately not done here to keep this change minimal and reversible.
        modelBuilder.Entity<Medicine>()
            .HasIndex(m => new { m.BrandName, m.Strength })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false")
            .HasDatabaseName("IX_Medicines_BrandName_Strength_Unique");

        modelBuilder.Entity<Manufacturer>()
            .HasIndex(m => m.Name)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false")
            .HasDatabaseName("IX_Manufacturers_Name_Unique");

        modelBuilder.Entity<Batch>()
            .HasIndex(b => new { b.MedicineId, b.BatchNo })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false")
            .HasDatabaseName("IX_Batches_MedicineId_BatchNo_Unique");


        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.SupplierCode)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false")
            .HasDatabaseName("IX_Suppliers_SupplierCode");
        // --- Query performance (non-unique) ---
        modelBuilder.Entity<Medicine>()
            .HasIndex(m => m.BrandName)
            .HasDatabaseName("IX_Medicines_BrandName");

        modelBuilder.Entity<Batch>()
            .HasIndex(b => b.ExpiryDate)
            .HasDatabaseName("IX_Batches_ExpiryDate"); // supports FEFO ordering on the Stock page

        modelBuilder.Entity<StockTransaction>()
            .HasIndex(st => st.BatchId)
            .HasDatabaseName("IX_StockTransactions_BatchId");

        modelBuilder.Entity<StockTransaction>()
            .HasIndex(st => st.MedicineId)
            .HasDatabaseName("IX_StockTransactions_MedicineId");

        modelBuilder.Entity<PurchaseItem>()
            .HasIndex(pi => pi.PurchaseId)
            .HasDatabaseName("IX_PurchaseItems_PurchaseId");

        modelBuilder.Entity<PurchaseItem>()
            .HasIndex(pi => pi.MedicineId)
            .HasDatabaseName("IX_PurchaseItems_MedicineId");

        modelBuilder.Entity<Purchase>()
            .HasIndex(p => p.SupplierId)
            .HasDatabaseName("IX_Purchases_SupplierId");

        modelBuilder.Entity<Purchase>()
            .HasIndex(p => p.PurchaseNumber)
            .IsUnique()
            .HasDatabaseName("IX_Purchases_PurchaseNumber_Unique");

        modelBuilder.Entity<Purchase>()
            .HasIndex(p => new { p.SupplierId, p.InvoiceNumber })
            .IsUnique()
            .HasFilter("\"Status\" <> 'Deleted'")
            .HasDatabaseName("IX_Purchases_SupplierId_InvoiceNumber_Unique");

        modelBuilder.Entity<Purchase>()
            .HasIndex(p => p.PurchaseDate)
            .HasDatabaseName("IX_Purchases_PurchaseDate");

        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.SupplierName)
            .HasDatabaseName("IX_Suppliers_SupplierName");

        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.MobileNumber)
            .HasDatabaseName("IX_Suppliers_MobileNumber");
        modelBuilder.Entity<Sale>()
            .HasIndex(s => s.InvoiceNumber)
            .IsUnique()
            .HasDatabaseName("IX_Sales_InvoiceNumber_Unique");

        modelBuilder.Entity<SaleItem>()
            .HasIndex(si => si.SaleId)
            .HasDatabaseName("IX_SaleItems_SaleId");

        modelBuilder.Entity<SaleItem>()
            .HasIndex(si => si.BatchId)
            .HasDatabaseName("IX_SaleItems_BatchId");
    }
}




