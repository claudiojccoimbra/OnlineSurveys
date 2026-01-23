using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OnlineSurveys.Infrastructure.Persistence;
using System.Text.Json.Serialization;
using OnlineSurveys.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

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

//
// SEED INICIAL – cria um questionário de exemplo se o banco estiver vazio
//
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SurveysDbContext>();

    // Garantir criação do banco InMemory
    db.Database.EnsureCreated();

    if (!db.Questionnaires.Any())
    {
        var questionnaire = new Questionnaire
        {
            Id = Guid.NewGuid(),
            Title = "Intenção de voto - Governador",
            Description = "Questionário de exemplo gerado automaticamente para demonstração.",
            StartsAt = DateTime.UtcNow.Date,
            EndsAt = DateTime.UtcNow.Date.AddDays(30),
            Questions =
            [
                new Question
                {
                    Id = Guid.NewGuid(),
                    Text = "Em quem você pretende votar para prefeito?",
                    Order = 1,
                    Choices =
                    [
                        new Choice { Id = Guid.NewGuid(), Text = "Candidato A", Order = 1 },
                        new Choice { Id = Guid.NewGuid(), Text = "Candidato B", Order = 2 },
                        new Choice { Id = Guid.NewGuid(), Text = "Branco/Nulo",   Order = 3 }
                    ]
                }
            ]
        };

        db.Questionnaires.Add(questionnaire);
        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineSurveys API v1");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
