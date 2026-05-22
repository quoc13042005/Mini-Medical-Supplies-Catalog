namespace MedicalSupplies.Mvc.Models;

public class Supply
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // Mã vật tư
    public string Name { get; set; } = string.Empty; // Tên vật tư
    public string Category { get; set; } = string.Empty; // Nhóm vật tư
    public string Provider { get; set; } = string.Empty; // Nhà cung cấp
    public decimal Price { get; set; } // Đơn giá
    public int Quantity { get; set; } // Số lượng tồn kho
    public int MinQuantity { get; set; } // Mức tồn tối thiểu
    public DateTime LastUpdated { get; set; } // Ngày cập nhật gần nhất

    public SupplyStatus Status
    {
        get
        {
            if (Quantity == 0) return SupplyStatus.OutOfStock;
            if (Quantity <= MinQuantity) return SupplyStatus.NeedsRestock;
            return SupplyStatus.InStock;
        }
    }
}
