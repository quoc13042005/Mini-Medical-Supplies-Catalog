using System.ComponentModel.DataAnnotations;

namespace MedicalSupplies.Mvc.Models;

public class Supply
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // Mã vật tư
    public string Barcode { get; set; } = string.Empty; // Mã vạch (Feature 3)
    public string Name { get; set; } = string.Empty; // Tên vật tư
    public int SupplyCategoryId { get; set; } // Khóa ngoại
    public SupplyCategory? Category { get; set; } // Navigation property
    public string Provider { get; set; } = string.Empty; // Nhà cung cấp
    public decimal Price { get; set; } // Đơn giá
    public int Quantity { get; set; } // Số lượng tồn kho
    public string? ImagePath { get; set; }
    public DateTime LastUpdated { get; set; } // Ngày cập nhật gần nhất

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
