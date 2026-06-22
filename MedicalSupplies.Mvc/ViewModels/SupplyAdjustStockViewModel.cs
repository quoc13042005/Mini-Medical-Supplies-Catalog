using System.ComponentModel.DataAnnotations;

namespace MedicalSupplies.Mvc.ViewModels;

public class SupplyAdjustStockViewModel
{
    public int Id { get; set; }

    [Display(Name = "Mã vật tư")]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "Tên vật tư")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Số lượng hiện tại")]
    public int CurrentQuantity { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số lượng thay đổi.")]
    [Display(Name = "Số lượng thay đổi (Nhập số âm để xuất kho)")]
    public int AdjustQuantity { get; set; }

    public string? RowVersion { get; set; }
}
