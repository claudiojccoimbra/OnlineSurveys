using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineSurveys.Infrastructure.Persistence;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Controllers
// Controllers + JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

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

// DbContext em memória (mock)
builder.Services.AddDbContext<SurveysDbContext>(options =>
    options.UseInMemoryDatabase("OnlineSurveysDb"));

var app = builder.Build();

// Swagger sempre habilitado
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineSurveys API v1");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
