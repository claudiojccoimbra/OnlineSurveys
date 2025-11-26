namespace OnlineSurveys.Domain.Entities;

public class Answer
{
    public Guid Id { get; set; }
    public Guid QuestionnaireId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid ChoiceId { get; set; }
    public DateTime AnsweredAt { get; set; }
    public string? RespondentSessionId { get; set; }   // para evitar duplicidade básica
}
