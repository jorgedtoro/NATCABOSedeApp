using Microsoft.EntityFrameworkCore;
using NATCABOSede.Services;
using NATCABOSede.Interfaces;
using System;
using ClosedXML.Parser;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


//**************
// Configure logging to include EventLog for IIS
builder.Logging.AddEventLog(settings =>
{
    settings.SourceName = "NATCABOSedeApp"; // Customize with your application name
});

Console.WriteLine($"Current Environment: {builder.Environment.EnvironmentName}");       //JMB
// Log the connection string for debugging
var connectionString = builder.Configuration.GetConnectionString("NATCABOConnection");
Console.WriteLine($"Connection String: {connectionString}");

//****************

// Add services to the container.
builder.Services.AddControllersWithViews();


// Register the database context with a custom CommandTimeout
builder.Services.AddDbContext<NATCABOSede.Models.NATCABOContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NATCABOConnection"), sqlServerOptions =>
    {
        sqlServerOptions.CommandTimeout(60); // Set timeout to 60 seconds
    }));
//Jorge --> servicio de Auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });


// Registro del servicio KPIService J.Toro
//builder.Services.AddScoped<NATCABOSede.Services.KPIService>();
builder.Services.AddScoped<IKPIService, KPIService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // Routing debe estar antes de los endpoints

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // Ruta para áreas
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    // Ruta por defecto (sin área)
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
//app.UseExceptionHandler("/Home/Error");     //JMB


app.Run();
