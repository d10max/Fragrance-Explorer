using FragranceExplorer.BLL.Handlers;
using FragranceExplorer.BLL.Providers;
using FragranceExplorer.BLL.Services;
using FragranceExplorer.BLL.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace FragranceExplorer.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBll(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddSingleton<NoteJaccardSimilarityStrategy>();
        services.AddSingleton<AccordCosineSimilarityStrategy>();

        services.AddSingleton<RecommendationEngine>(sp =>
        {
            var accordStrategy = sp.GetRequiredService<AccordCosineSimilarityStrategy>();
            var noteStrategy = sp.GetRequiredService<NoteJaccardSimilarityStrategy>();

            return new RecommendationEngine(accordStrategy, noteStrategy);
        });

        services.AddSingleton<IPerfumeSearchService, PerfumeSearchService>();

        services.AddScoped<PerfumeMathProvider>();
        services.AddScoped<GetPerfumeRecommendationsHandler>();
        services.AddScoped<PerfumeSearchHandler>();
        return services;
    }
}
