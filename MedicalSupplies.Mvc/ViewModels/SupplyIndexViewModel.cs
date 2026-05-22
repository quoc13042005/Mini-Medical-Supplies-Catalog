using MedicalSupplies.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalSupplies.Mvc.ViewModels;

public class SupplyIndexViewModel
{
    public IEnumerable<SupplyListItemViewModel> Supplies { get; set; } = new List<SupplyListItemViewModel>();
    
    // Tìm kiếm và lọc
    public string? SearchString { get; set; }
    public string? Category { get; set; }
    public SupplyStatus? Status { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> StatusList { get; set; } = new List<SelectListItem>();

    // Phân trang
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10; // Đổi mặc định thành 10 item/trang
}
