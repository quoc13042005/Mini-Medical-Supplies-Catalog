using MedicalSupplies.Mvc.Repositories;
using MedicalSupplies.Mvc.Options;
using MedicalSupplies.Mvc.ViewModels;
using MedicalSupplies.Mvc.Models;
using Microsoft.Extensions.Options;

namespace MedicalSupplies.Mvc.Services;

public class SupplyService : ISupplyService
{
    private readonly ISupplyRepository _supplyRepository;
    private readonly AppSettings _settings;

    public SupplyService(ISupplyRepository supplyRepository, IOptions<AppSettings> options)
    {
        _supplyRepository = supplyRepository;
        _settings = options.Value;
    }

    public async Task<List<SupplyListItemViewModel>> GetSupplyListAsync()
    {
        var supplies = await _supplyRepository.GetAllReadOnlyAsync();
        return MapToViewModel(supplies);
    }

    public async Task<List<SupplyListItemViewModel>> FilterSuppliesAsync(int? categoryId, decimal? minPrice, decimal? maxPrice)
    {
        var supplies = await _supplyRepository.FilterSuppliesAsync(categoryId, minPrice, maxPrice);
        return MapToViewModel(supplies);
    }

    private List<SupplyListItemViewModel> MapToViewModel(List<Supply> supplies)
    {
        return supplies.Select(s => new SupplyListItemViewModel
        {
            Id = s.Id,
            Code = s.Code,
            Barcode = s.Barcode,
            Name = s.Name,
            Category = s.Category?.Name ?? "N/A",
            Provider = s.Provider,
            Price = s.Price,
            Quantity = s.Quantity,
            Status = GetStatus(s.Quantity)
        }).ToList();
    }

    private SupplyStatus GetStatus(int quantity)
    {
        if (quantity == 0) return SupplyStatus.OutOfStock;
        if (quantity <= _settings.LowStockThreshold) return SupplyStatus.NeedsRestock;
        return SupplyStatus.InStock;
    }
}
