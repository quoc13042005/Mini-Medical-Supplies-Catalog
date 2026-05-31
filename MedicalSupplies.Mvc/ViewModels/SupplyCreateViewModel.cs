using System.ComponentModel.DataAnnotations;

namespace MedicalSupplies.Mvc.ViewModels;

public class SupplyCreateViewModel
{
    [Required(ErrorMessage = "Mã vật tư không được để trống")]
    public string Code { get; set; } = "";

    [Required(ErrorMessage = "Tên vật tư không được để trống")]
    [StringLength(100, ErrorMessage = "Tên vật tư không được vượt quá 100 ký tự")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Nhóm vật tư không được để trống")]
    public string Category { get; set; } = "";

    [Required(ErrorMessage = "Nhà cung cấp không được để trống")]
    public string Provider { get; set; } = "";

    [Range(1, 100000000, ErrorMessage = "Đơn giá phải lớn hơn 0")]
    public decimal Price { get; set; }

    [Range(0, 100000, ErrorMessage = "Số lượng tồn không được âm")]
    public int Quantity { get; set; }

    [Range(0, 10000, ErrorMessage = "Mức tồn tối thiểu không được âm")]
    public int MinQuantity { get; set; }
}
