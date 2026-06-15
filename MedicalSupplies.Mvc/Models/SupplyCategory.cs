namespace MedicalSupplies.Mvc.Models;

public class SupplyCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Supply> Supplies { get; set; } = new List<Supply>();
}
