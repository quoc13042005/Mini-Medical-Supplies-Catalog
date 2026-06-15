using MedicalSupplies.Mvc.Data;
using MedicalSupplies.Mvc.Models;
using MedicalSupplies.Mvc.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalSupplies.Mvc.Services;

public class IssueService : IIssueService
{
    private readonly AppDbContext _context;

    public IssueService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateIssueAsync(IssueCreateViewModel model)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var supply = await _context.Supplies.FirstOrDefaultAsync(s => s.Id == model.SupplyId);
            if (supply == null) throw new Exception("Không tìm thấy vật tư.");
            if (supply.Quantity < model.Quantity) throw new Exception("Không đủ số lượng tồn kho.");

            var issue = new Issue
            {
                IssueDate = DateTime.Now,
                ReceiverName = model.ReceiverName,
                TotalAmount = supply.Price * model.Quantity
            };
            
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            var item = new IssueItem
            {
                IssueId = issue.Id,
                SupplyId = supply.Id,
                Quantity = model.Quantity,
                UnitPrice = supply.Price
            };
            
            _context.IssueItems.Add(item);
            
            supply.Quantity -= model.Quantity;
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
