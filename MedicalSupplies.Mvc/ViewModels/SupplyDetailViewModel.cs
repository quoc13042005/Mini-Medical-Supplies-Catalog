using MedicalSupplies.Mvc.Models;

namespace MedicalSupplies.Mvc.ViewModels;

public class SupplyDetailViewModel
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int MinQuantity { get; set; }
    public SupplyStatus Status { get; set; }
    public DateTime LastUpdated { get; set; }
}
