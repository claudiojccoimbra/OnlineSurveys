using Microsoft.EntityFrameworkCore;
using OnlineSurveys.Domain.Entities;

namespace OnlineSurveys.Infrastructure.Persistence;

public class SurveysDbContext : DbContext
{
    public SurveysDbContext(DbContextOptions<SurveysDbContext> options) : base(options)
    {
    }

    public DbSet<Questionnaire> Questionnaires => Set<Questionnaire>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Choice> Choices => Set<Choice>();
    public DbSet<Answer> Answers => Set<Answer>();
}
