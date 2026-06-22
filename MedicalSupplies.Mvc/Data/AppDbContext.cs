using MedicalSupplies.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalSupplies.Mvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SupplyCategory> SupplyCategories => Set<SupplyCategory>();
    public DbSet<Supply> Supplies => Set<Supply>();
    public DbSet<Issue> Issues => Set<Issue>();
    public DbSet<IssueItem> IssueItems => Set<IssueItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SupplyCategory>(entity =>
        {
            entity.ToTable("SupplyCategories");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            
            entity.HasData(
                new SupplyCategory { Id = 1, Name = "Vật tư tiêu hao" },
                new SupplyCategory { Id = 2, Name = "Dụng cụ tiêm truyền" },
                new SupplyCategory { Id = 3, Name = "Thiết bị y tế" }
            );
        });

        modelBuilder.Entity<Supply>(entity =>
        {
            entity.ToTable("Supplies");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Code).IsRequired().HasMaxLength(50);
            entity.Property(s => s.Barcode).HasMaxLength(50);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
            entity.Property(s => s.Provider).HasMaxLength(100);
            entity.Property(s => s.Price).HasColumnType("decimal(18,2)");
            
            entity.HasOne(s => s.Category)
                  .WithMany(c => c.Supplies)
                  .HasForeignKey(s => s.SupplyCategoryId);

            entity.HasIndex(s => s.Code).IsUnique();
            entity.Property(s => s.RowVersion).IsConcurrencyToken();
            entity.HasQueryFilter(s => !s.IsDeleted);

            entity.HasData(
                new Supply { Id = 1, SupplyCategoryId = 1, Code = "MS001", Barcode = "893001", Name = "Khẩu trang y tế 4 lớp", Provider = "MediPlast", Price = 50000, Quantity = 1000, LastUpdated = new DateTime(2023, 1, 1) },
                new Supply { Id = 2, SupplyCategoryId = 1, Code = "MS002", Barcode = "893002", Name = "Găng tay y tế Nitrile", Provider = "VGlove", Price = 80000, Quantity = 500, LastUpdated = new DateTime(2023, 1, 1) },
                new Supply { Id = 3, SupplyCategoryId = 2, Code = "MS003", Barcode = "893003", Name = "Bơm tiêm nhựa 5ml", Provider = "Vinahoc", Price = 1500, Quantity = 0, LastUpdated = new DateTime(2023, 1, 1) },
                new Supply { Id = 4, SupplyCategoryId = 2, Code = "MS004", Barcode = "893004", Name = "Dây truyền dịch", Provider = "Danapha", Price = 8000, Quantity = 15, LastUpdated = new DateTime(2023, 1, 1) },
                new Supply { Id = 5, SupplyCategoryId = 3, Code = "MS005", Barcode = "893005", Name = "Máy đo huyết áp điện tử", Provider = "Omron", Price = 1200000, Quantity = 15, LastUpdated = new DateTime(2023, 1, 1) }
            );
        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.ToTable("Issues");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ReceiverName).IsRequired().HasMaxLength(150);
            entity.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<IssueItem>(entity =>
        {
            entity.ToTable("IssueItems");
            entity.HasKey(i => i.Id);
            entity.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");

            entity.HasOne(i => i.Issue)
                  .WithMany(i => i.IssueItems)
                  .HasForeignKey(i => i.IssueId);

            entity.HasOne(i => i.Supply)
                  .WithMany()
                  .HasForeignKey(i => i.SupplyId);
        });
    }
}
