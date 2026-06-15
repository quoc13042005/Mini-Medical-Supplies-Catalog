using System.ComponentModel.DataAnnotations;

namespace MedicalSupplies.Mvc.ViewModels;

public class IssueCreateViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên người/đơn vị nhận.")]
    public string ReceiverName { get; set; } = string.Empty;
    
    [Required]
    public int SupplyId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng xuất phải lớn hơn 0")]
    public int Quantity { get; set; }
}
