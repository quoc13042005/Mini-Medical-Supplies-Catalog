using Microsoft.EntityFrameworkCore;
using MedicalSupplies.Mvc.Models;
using MedicalSupplies.Mvc.Data;

namespace MedicalSupplies.Mvc.Repositories;

public class SupplyRepository : ISupplyRepository
{
    private readonly AppDbContext _context;

    public SupplyRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Supply>> GetAllAsync()
        => _context.Supplies.Include(s => s.Category).ToListAsync();

    public Task<List<Supply>> GetAllReadOnlyAsync()
        => _context.Supplies.Include(s => s.Category).AsNoTracking().ToListAsync();

    public Task<Supply?> GetByIdAsync(int id)
        => _context.Supplies.Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == id);

    public async Task AddAsync(Supply supply)
        => await _context.Supplies.AddAsync(supply);

    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();

    public Task<List<Supply>> FilterSuppliesAsync(int? categoryId, decimal? minPrice, decimal? maxPrice)
    {
        var query = _context.Supplies.Include(s => s.Category).AsNoTracking();

        if (categoryId.HasValue)
        {
            query = query.Where(s => s.SupplyCategoryId == categoryId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(s => s.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(s => s.Price <= maxPrice.Value);
        }

        return query.ToListAsync();
    }
}
