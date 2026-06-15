using MedicalSupplies.Mvc.ViewModels;

namespace MedicalSupplies.Mvc.Services;

public interface ISupplyService
{
    Task<List<SupplyListItemViewModel>> GetSupplyListAsync();
    Task<List<SupplyListItemViewModel>> FilterSuppliesAsync(int? categoryId, decimal? minPrice, decimal? maxPrice);
}
