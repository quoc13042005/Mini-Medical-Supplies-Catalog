using MedicalSupplies.Mvc.Models;

namespace MedicalSupplies.Mvc.Repositories;

public interface IIssueRepository
{
    Task AddAsync(Issue issue);
}
