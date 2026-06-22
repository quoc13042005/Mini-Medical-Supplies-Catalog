using System.ComponentModel.DataAnnotations;

namespace MedicalSupplies.Mvc.ViewModels;

public class SupplyEditViewModel : SupplyCreateViewModel
{
    public int Id { get; set; }
    public string? RowVersion { get; set; }
}
