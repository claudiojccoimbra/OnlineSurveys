using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineSurveys.Infrastructure.Persistence;

public class AggregationWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public AggregationWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<SurveysDbContext>();

            // TODO: ler respostas novas e atualizar resumos

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
