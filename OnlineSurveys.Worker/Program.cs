using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineSurveys.Infrastructure.Persistence;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<SurveysDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("SurveysDb")));

        services.AddHostedService<AggregationWorker>();
    })
    .Build()
    .Run();
