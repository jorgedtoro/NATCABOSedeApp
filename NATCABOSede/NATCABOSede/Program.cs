using Microsoft.EntityFrameworkCore;
using NATCABOSede.Services;
using NATCABOSede.Interfaces;
using NATCABOSede.Models;
using System;
using ClosedXML.Parser;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

// Cargar configuración adicional basada en el sistema operativo
if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    builder.Configuration.AddJsonFile("appsettings.Development.Mac.json", optional: true, reloadOnChange: true);
    Console.WriteLine("Cargando configuración para macOS/Linux");
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    // Configuración específica de Windows
    builder.Logging.AddEventLog(settings =>
    {
        settings.SourceName = "NATCABOSedeApp";
    });
    Console.WriteLine("Cargando configuración para Windows");
}

// Configuración de logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Mostrar información del entorno
Console.WriteLine($"Current Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"OS: {RuntimeInformation.OSDescription}");
Console.WriteLine($"Database Connection: {builder.Configuration.GetConnectionString("NATCABOConnection")}");

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configuración de la base de datos
var connectionString = builder.Configuration.GetConnectionString("NATCABOConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No se ha configurado la cadena de conexión 'NATCABOConnection'");
}

Console.WriteLine($"Using connection string: {connectionString}");

// Configurar el contexto de la base de datos
builder.Services.AddDbContext<NATCABOSede.Models.NATCABOContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.CommandTimeout(60); // Timeout de 60 segundos
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });

    // Solo habilitar logging detallado en desarrollo
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine, LogLevel.Information);
    }
});
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
