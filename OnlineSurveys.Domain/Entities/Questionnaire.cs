using System.ComponentModel.DataAnnotations;

namespace OnlineSurveys.Domain.Entities;

public class Questionnaire
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime StartsAt { get; set; }

    public DateTime EndsAt { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

public class Question
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid QuestionnaireId { get; set; }

    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Ordem da pergunta dentro do questionário.
    /// </summary>
    public int Order { get; set; }

    public Questionnaire Questionnaire { get; set; } = null!;

    public ICollection<Choice> Choices { get; set; } = new List<Choice>();
}

public class Choice
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid QuestionId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Ordem da alternativa dentro da pergunta.
    /// </summary>
    public int Order { get; set; }

    public Question Question { get; set; } = null!;
}
