using Microsoft.EntityFrameworkCore;
using NATCABOSede.Services;
using NATCABOSede.Interfaces;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//TODO: Registrar el contexto de base de datos
builder.Services.AddDbContext<NATCABOSede.Models.NATCABOContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NATCABOConnection")));


// Registro del servicio KPIService J.Toro
//builder.Services.AddScoped<NATCABOSede.Services.KPIService>();
builder.Services.AddScoped<IKPIService, KPIService>();

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
    // Rutas para las áreas
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    // Ruta por defecto
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

//app.UseExceptionHandler("/Home/Error");     //JMB


app.Run();
