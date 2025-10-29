using Microsoft.EntityFrameworkCore;
using CarCollection.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure EF Core - SQLite file
var connString = builder.Configuration.GetConnectionString("DefaultConnection") 
                 ?? "Data Source=carcollection.db";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connString));

var app = builder.Build();

// Ensure upload folder exists
var uploads = Path.Combine(app.Environment.WebRootPath ?? "wwwroot", "uploads");
Directory.CreateDirectory(uploads);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cars}/{action=Index}/{id?}");

app.Run();
