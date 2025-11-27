using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineSurveys.Domain.Entities;
using OnlineSurveys.Infrastructure.Persistence;

namespace OnlineSurveys.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionnairesController : ControllerBase
    {
        private readonly SurveysDbContext _db;

        public QuestionnairesController(SurveysDbContext db)
        {
            _db = db;
        }

        // GET: api/questionnaires
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Questionnaire>>> GetAll()
        {
            var questionnaires = await _db.Questionnaires
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Choices)
                .ToListAsync();

            return Ok(questionnaires);
        }

        // GET: api/questionnaires/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Questionnaire>> GetById(Guid id)
        {
            var questionnaire = await _db.Questionnaires
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (questionnaire == null)
                return NotFound();

            return Ok(questionnaire);
        }

        // POST: api/questionnaires
        [HttpPost]
        public async Task<ActionResult<Questionnaire>> Create(CreateQuestionnaireRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var questionnaire = new Questionnaire
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                StartsAt = request.StartsAt,
                EndsAt = request.EndsAt,
                Questions = request.Questions.Select((qDto, index) => new Question
                {
                    Id = Guid.NewGuid(),
                    Text = qDto.Text,
                    Order = qDto.Order ?? (index + 1),
                    Choices = qDto.Choices.Select((cDto, cIndex) => new Choice
                    {
                        Id = Guid.NewGuid(),
                        Text = cDto.Text,
                        Order = cDto.Order ?? (cIndex + 1)
                    }).ToList()
                }).ToList()
            };

            _db.Questionnaires.Add(questionnaire);
            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = questionnaire.Id },
                questionnaire);
        }
    }

    // DTOs simples para criação de questionário
    public class CreateQuestionnaireRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public List<CreateQuestionRequest> Questions { get; set; } = new();
    }

    public class CreateQuestionRequest
    {
        public string Text { get; set; } = string.Empty;
        public int? Order { get; set; }
        public List<CreateChoiceRequest> Choices { get; set; } = new();
    }

    public class CreateChoiceRequest
    {
        public string Text { get; set; } = string.Empty;
        public int? Order { get; set; }
    }
}
