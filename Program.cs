using Microsoft.EntityFrameworkCore;
using RailwayBookingApp.Models;

using RailwayBookingApp.Data;
using RailwayBookingApp.Services;
using RailwayBookingApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Default"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"))
    ));

// ✅ Add cache for session
builder.Services.AddDistributedMemoryCache();

// ✅ Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<ITrainService, TrainService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<EmailService>();


var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

// ✅ IMPORTANT ORDER
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();