using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using OnlineSurveys.Web.Models;

namespace OnlineSurveys.Web.Controllers
{
    public class SurveysController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public SurveysController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("Backend");

            List<QuestionnaireViewModel> questionnaires = new();

            try
            {
                var response = await client.GetAsync("api/questionnaires");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<List<QuestionnaireViewModel>>();
                    if (data is not null)
                        questionnaires = data;
                }
                else
                {
                    // em um sistema real: logar o erro / mostrar mensagem amigável
                    ViewBag.Error = $"Falha ao consultar API: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                // em um sistema real: logar
                ViewBag.Error = $"Erro ao chamar API: {ex.Message}";
            }

            return View(questionnaires);
        }
    }
}
