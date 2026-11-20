using FragranceExplorer.BLL.Parser.Houses.Interfaces;
using FragranceExplorer.BLL.Parser.Houses.Services;
using FragranceExplorer.BLL.Parser.Perfumes.Interfaces;
using FragranceExplorer.BLL.Parser.Perfumes.Services;
using FragranceExplorer.BLL.Repositories;
using FragranceExplorer.BLL.Services;
using FragranceExplorer.BLL.Strategies;
using FragranceExplorer.DAL;

namespace FragranceExplorer_Back;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDal(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IPerfumeDataSetParser, PerfumeDataSetParser>();
        builder.Services.AddSingleton<IHouseDataSetParser, HouseDataSetParser>();

        builder.Services.AddSingleton<InMemoryPerfumeRepository>();
        builder.Services.AddSingleton<IPerfumeRepository>(sp => sp.GetRequiredService<InMemoryPerfumeRepository>());

        builder.Services.AddSingleton<NoteJaccardSimilarityStrategy>();
        builder.Services.AddSingleton<AccordCosineSimilarityStrategy>();

        builder.Services.AddSingleton<RecommendationEngine>(sp =>
        {
            var accordStrategy = sp.GetRequiredService<AccordCosineSimilarityStrategy>();
            var noteStrategy = sp.GetRequiredService<NoteJaccardSimilarityStrategy>();

            return new RecommendationEngine(accordStrategy, noteStrategy);
        });

        builder.Services.AddSingleton<IPerfumeSearchService, PerfumeSearchService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<InMemoryPerfumeRepository>();

            Console.WriteLine("Starting dataset load into memory.");

            await repository.InitializeAsync();

            Console.WriteLine("Dataset loaded successfully.");
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
