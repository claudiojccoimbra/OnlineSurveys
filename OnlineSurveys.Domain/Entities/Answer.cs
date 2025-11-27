using System.ComponentModel.DataAnnotations;

namespace OnlineSurveys.Domain.Entities;

public class Answer
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid QuestionnaireId { get; set; }

    [Required]
    public Guid QuestionId { get; set; }

    [Required]
    public Guid ChoiceId { get; set; }

    public DateTime AnsweredAt { get; set; }

    /// <summary>
    /// Algum identificador leve do respondente (session, cookie, etc.)
    /// apenas para evitar duplicidade básica.
    /// </summary>
    [MaxLength(200)]
    public string? RespondentSessionId { get; set; }

    public Questionnaire Questionnaire { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public Choice Choice { get; set; } = null!;
}
