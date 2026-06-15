using MedicalSupplies.Mvc.Models;

namespace MedicalSupplies.Mvc.Repositories;

public interface ISupplyRepository
{
    Task<List<Supply>> GetAllAsync();
    Task<List<Supply>> GetAllReadOnlyAsync();
    Task<Supply?> GetByIdAsync(int id);
    Task AddAsync(Supply supply);
    Task SaveChangesAsync();
    Task<List<Supply>> FilterSuppliesAsync(int? categoryId, decimal? minPrice, decimal? maxPrice);
}
