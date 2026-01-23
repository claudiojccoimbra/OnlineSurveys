using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// HttpClient para consumir a API
builder.Services.AddHttpClient("OnlineSurveysApi", client =>
{
    // Base da API (ajusta a porta, se precisar)
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5222");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Rota padrão: Surveys/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Surveys}/{action=Index}/{id?}");

app.Run();
