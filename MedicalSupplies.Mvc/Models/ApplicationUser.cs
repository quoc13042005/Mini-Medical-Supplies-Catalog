using Microsoft.AspNetCore.Identity;

namespace MedicalSupplies.Mvc.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
