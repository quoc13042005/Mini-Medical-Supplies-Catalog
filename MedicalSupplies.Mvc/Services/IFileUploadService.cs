using Microsoft.AspNetCore.Http;

namespace MedicalSupplies.Mvc.Services;

public interface IFileUploadService
{
    Task<string> SaveProductImageAsync(IFormFile file);
    void DeleteImage(string imagePath);
}
