using MedicalSupplies.Mvc.Data;
using MedicalSupplies.Mvc.Options;
using MedicalSupplies.Mvc.Repositories;
using MedicalSupplies.Mvc.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MedicalSupplies.Mvc.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File($"logs/lab05-{DateTime.Now:yyyyMMdd}.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllersWithViews();

// Add ProblemDetails and HealthChecks
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("database_ready_check");

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewProduct", p => p.RequireRole("Admin", "Staff"));
    options.AddPolicy("CanManageProduct", p => p.RequireRole("Admin"));
    options.AddPolicy("CanAdjustStock", p => p.RequireRole("Admin", "Staff"));
    options.AddPolicy("CanUploadProductImage", p => p.RequireRole("Admin"));
    options.AddPolicy("CanViewAuditLog", p => p.RequireRole("Admin"));
});

builder.Services.AddScoped<ISupplyRepository, SupplyRepository>();
builder.Services.AddScoped<ISupplyService, SupplyService>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map HealthChecks
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false // Only check if the app is responsive
});
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => true // Check all including DB
});

// Map API Error Demo
app.MapGet("/api/supplies/{id:int}", async (int id, AppDbContext db, HttpContext http) =>
{
    var supply = await db.Supplies.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    if (supply == null || supply.IsDeleted)
    {
        return Results.Problem(
            type: "https://example.com/problems/supply-not-found",
            title: "Supply not found",
            detail: $"The supply with id {id} was not found.",
            statusCode: StatusCodes.Status404NotFound,
            instance: http.Request.Path);
    }
    return Results.Ok(supply);
});

// Map API Search Demo
app.MapGet("/api/supplies/search", async (string? keyword, AppDbContext db, HttpContext http) =>
{
    if (string.IsNullOrWhiteSpace(keyword) || keyword.Length > 50)
    {
        var errors = new Dictionary<string, string[]>
        {
            { "keyword", new[] { "Keyword is required and must not exceed 50 characters." } }
        };
        return Results.ValidationProblem(errors, statusCode: StatusCodes.Status400BadRequest, title: "Invalid search keyword");
    }

    var supplies = await db.Supplies.AsNoTracking().Where(s => s.Name.Contains(keyword) || s.Code.Contains(keyword)).ToListAsync();

    if (!supplies.Any())
    {
        return Results.Problem(
            type: "https://example.com/problems/search-no-results",
            title: "No results found",
            detail: $"No supplies found for keyword '{keyword}'.",
            statusCode: StatusCodes.Status404NotFound,
            instance: http.Request.Path,
            extensions: new Dictionary<string, object?> { { "errorCode", "SUPPLIES_NOT_FOUND" } });
    }

    return Results.Ok(supplies);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedIdentityAsync(scope.ServiceProvider);
}

app.Run();
