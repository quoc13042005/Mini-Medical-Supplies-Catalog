using MedicalSupplies.Mvc.Models;
using MedicalSupplies.Mvc.Data;

namespace MedicalSupplies.Mvc.Repositories;

public class IssueRepository : IIssueRepository
{
    private readonly AppDbContext _context;

    public IssueRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Issue issue)
    {
        await _context.Issues.AddAsync(issue);
    }
}
