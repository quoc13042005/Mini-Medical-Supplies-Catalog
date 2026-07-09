namespace MedicalSupplies.Mvc.ViewModels;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public int TotalAuditLogs { get; set; }
    
    // Security metrics for today
    public int AccessDeniedCountToday { get; set; }
    public int SensitiveActionsToday { get; set; }
    public int UploadDeniedCountToday { get; set; }
}
