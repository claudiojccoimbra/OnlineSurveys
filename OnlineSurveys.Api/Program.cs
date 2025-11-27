using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineSurveys.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OnlineSurveys API",
        Version = "v1",
        Description = "API para criação de questionários, registro de respostas e consulta de resultados."
    });
});

// DbContext
builder.Services.AddDbContext<SurveysDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SurveysDb")));

var app = builder.Build();

// Swagger só em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineSurveys API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
