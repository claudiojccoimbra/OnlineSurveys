namespace OnlineSurveys.Domain.Entities;

public class Questionnaire
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

public class Question
{
    public Guid Id { get; set; }
    public Guid QuestionnaireId { get; set; }
    public string Text { get; set; } = null!;
    public int Order { get; set; }

    public Questionnaire Questionnaire { get; set; } = null!;
    public ICollection<Choice> Choices { get; set; } = new List<Choice>();
}

public class Choice
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public string Text { get; set; } = null!;
    public int Order { get; set; }

    public Question Question { get; set; } = null!;
}
