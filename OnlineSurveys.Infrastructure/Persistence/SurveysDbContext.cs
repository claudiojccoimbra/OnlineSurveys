using Microsoft.EntityFrameworkCore;
using OnlineSurveys.Domain.Entities;

namespace OnlineSurveys.Infrastructure.Persistence
{
    public class SurveysDbContext : DbContext
    {
        public SurveysDbContext(DbContextOptions<SurveysDbContext> options)
            : base(options)
        {
        }

        public DbSet<Questionnaire> Questionnaires => Set<Questionnaire>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Choice> Choices => Set<Choice>();
        public DbSet<Answer> Answers => Set<Answer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Questionnaire
            modelBuilder.Entity<Questionnaire>(entity =>
            {
                entity.HasKey(q => q.Id);

                entity.Property(q => q.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(q => q.Description)
                    .HasMaxLength(1000);

                entity.Property(q => q.StartsAt)
                    .IsRequired();

                entity.Property(q => q.EndsAt)
                    .IsRequired();

                entity.HasMany(q => q.Questions)
                    .WithOne(qn => qn.Questionnaire)
                    .HasForeignKey(qn => qn.QuestionnaireId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Question
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.Id);

                entity.Property(q => q.Text)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(q => q.Order)
                    .IsRequired();

                entity.HasMany(q => q.Choices)
                    .WithOne(c => c.Question)
                    .HasForeignKey(c => c.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Choice
            modelBuilder.Entity<Choice>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Text)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(c => c.Order)
                    .IsRequired();
            });

            // Answer
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.AnsweredAt)
                    .IsRequired();

                entity.Property(a => a.RespondentSessionId)
                    .HasMaxLength(200);

                entity.HasOne(a => a.Questionnaire)
                    .WithMany()
                    .HasForeignKey(a => a.QuestionnaireId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Question)
                    .WithMany()
                    .HasForeignKey(a => a.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Choice)
                    .WithMany()
                    .HasForeignKey(a => a.ChoiceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(a => new { a.QuestionnaireId, a.QuestionId, a.ChoiceId });
            });
        }
    }
}
