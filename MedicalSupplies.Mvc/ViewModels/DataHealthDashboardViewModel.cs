namespace MedicalSupplies.Mvc.ViewModels;

public class DataHealthDashboardViewModel
{
    public int TotalActiveSupplies { get; set; }
    public int TotalDeletedSupplies { get; set; }
    public int TotalCategories { get; set; }
    public int OutOfStockSupplies { get; set; }
    
    // Some stats to make the dashboard look nice
    public decimal TotalInventoryValue { get; set; }
    
    // DB stats
    public string DatabaseProvider { get; set; } = "SQLite";
    public string ConnectionStatus { get; set; } = "Healthy";
    public DateTime LastChecked { get; set; } = DateTime.Now;
}
