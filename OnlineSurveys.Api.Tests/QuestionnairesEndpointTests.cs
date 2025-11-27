using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineSurveys.Api.Controllers;
using OnlineSurveys.Domain.Entities;
using OnlineSurveys.Infrastructure.Persistence;

namespace OnlineSurveys.Api.Tests
{
    public class QuestionnairesControllerTests
    {
        private SurveysDbContext BuildInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<SurveysDbContext>()
                // Usa um nome de banco único por teste
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new SurveysDbContext(options);
        }

        [Fact]
        public async Task GetAll_DeveRetornarOkComListaDeQuestionarios()
        {
            // Arrange
            using var db = BuildInMemoryDb();

            db.Questionnaires.Add(new Questionnaire
            {
                Id = Guid.NewGuid(),
                Title = "Pesquisa Teste",
                StartsAt = DateTime.UtcNow,
                EndsAt = DateTime.UtcNow.AddDays(1)
            });

            await db.SaveChangesAsync();

            var controller = new QuestionnairesController(db);

            // Act
            var result = await controller.GetAll();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var lista = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<Questionnaire>>(ok.Value);
            Assert.Single(lista);
        }

        [Fact]
        public async Task Create_DeveSalvarQuestionarioERetornarCreated()
        {
            // Arrange
            using var db = BuildInMemoryDb();
            var controller = new QuestionnairesController(db);

            var request = new CreateQuestionnaireRequest
            {
                Title = "Intenção de voto - Prefeito",
                Description = "Pesquisa teste",
                StartsAt = DateTime.UtcNow,
                EndsAt = DateTime.UtcNow.AddDays(10),
                Questions =
                {
                    new CreateQuestionRequest
                    {
                        Text = "Em quem você pretende votar?",
                        Choices =
                        {
                            new CreateChoiceRequest { Text = "Candidato A" },
                            new CreateChoiceRequest { Text = "Candidato B" },
                            new CreateChoiceRequest { Text = "Branco/Nulo" }
                        }
                    }
                }
            };

            // Act
            var actionResult = await controller.Create(request);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var questionnaire = Assert.IsType<Questionnaire>(created.Value);

            Assert.Equal("Intenção de voto - Prefeito", questionnaire.Title);
            Assert.Equal(1, db.Questionnaires.Count());
            Assert.Equal(questionnaire.Id, db.Questionnaires.Single().Id);
            Assert.Single(questionnaire.Questions);
            Assert.Equal(3, questionnaire.Questions.First().Choices.Count);
        }
    }
}
