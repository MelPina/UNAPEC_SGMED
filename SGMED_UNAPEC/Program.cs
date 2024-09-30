using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SGMED_UNAPEC.Data;
using SGMED_UNAPEC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<sgmed_unapecContext>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("conexion"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.0.1-mysql")));


builder.Services.AddScoped<DA_Usuario>();



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Acceso/Index";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        option.AccessDeniedPath = "/Home/Privacy";
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting(); 
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Index}/{id?}");



app.Run();
