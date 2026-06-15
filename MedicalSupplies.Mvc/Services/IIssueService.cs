using MedicalSupplies.Mvc.ViewModels;

namespace MedicalSupplies.Mvc.Services;

public interface IIssueService
{
    Task CreateIssueAsync(IssueCreateViewModel model);
}
