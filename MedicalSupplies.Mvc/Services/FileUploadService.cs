using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MedicalSupplies.Mvc.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _environment;

    public FileUploadService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveProductImageAsync(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(ext))
        {
            throw new InvalidOperationException("File type is not allowed. Only .jpg, .jpeg, .png, .webp are allowed.");
        }
        
        if (file.Length > 2 * 1024 * 1024)
        {
            throw new InvalidOperationException("File is too large. Maximum size is 2MB.");
        }

        var safeName = $"{Guid.NewGuid():N}{ext}";
        var folder = Path.Combine(_environment.WebRootPath, "uploads", "products");
        Directory.CreateDirectory(folder);
        var path = Path.Combine(folder, safeName);

        using var stream = new FileStream(path, FileMode.CreateNew);
        await file.CopyToAsync(stream);

        return $"/uploads/products/{safeName}";
    }

    public void DeleteImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return;
        
        var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
