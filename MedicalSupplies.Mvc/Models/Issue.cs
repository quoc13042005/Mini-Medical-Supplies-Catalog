namespace MedicalSupplies.Mvc.Models;

public class Issue
{
    public int Id { get; set; }
    public DateTime IssueDate { get; set; } = DateTime.Now;
    public string ReceiverName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }

    public ICollection<IssueItem> IssueItems { get; set; } = new List<IssueItem>();
}
