using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using OnlineSurveys.Web.Models;

namespace OnlineSurveys.Web.Controllers;

public class SurveysController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public SurveysController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _clientFactory.CreateClient("OnlineSurveysApi");

        // Chama GET /api/Questionnaires na API
        var questionnaires =
            await client.GetFromJsonAsync<List<QuestionnaireViewModel>>("api/Questionnaires")
            ?? new List<QuestionnaireViewModel>();

        return View(questionnaires);
    }
}
