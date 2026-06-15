namespace MedicalSupplies.Mvc.Models;

public class IssueItem
{
    public int Id { get; set; }
    
    public int IssueId { get; set; }
    public Issue? Issue { get; set; }

    public int SupplyId { get; set; }
    public Supply? Supply { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
