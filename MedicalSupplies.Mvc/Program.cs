using MedicalSupplies.Mvc.Data;
using MedicalSupplies.Mvc.Options;
using MedicalSupplies.Mvc.Repositories;
using MedicalSupplies.Mvc.Services;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddHealthChecks();

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISupplyRepository, SupplyRepository>();
builder.Services.AddScoped<ISupplyService, SupplyService>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();
builder.Services.AddScoped<IIssueService, IssueService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Map HealthChecks
app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
