using System.ComponentModel.DataAnnotations;

namespace MedicalSupplies.Mvc.ViewModels;

public class SupplyCreateViewModel
{
    [Required(ErrorMessage = "Mã vật tư là bắt buộc.")]
    [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "Mã vật tư chỉ gồm chữ in hoa, số và dấu -.")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên vật tư là bắt buộc.")]
    [StringLength(200, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    public string? Barcode { get; set; }

    [Required(ErrorMessage = "Danh mục là bắt buộc.")]
    public int SupplyCategoryId { get; set; }

    public string? Provider { get; set; }

    [Range(1000, 100000000, ErrorMessage = "Giá phải từ 1.000 đến 100.000.000.")]
    public decimal Price { get; set; }

    [Range(0, 100000, ErrorMessage = "Số lượng phải từ 0 đến 100.000.")]
    public int Quantity { get; set; }
}
