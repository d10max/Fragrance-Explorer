using FragranceExplorer.BLL.Parser.Common;
using FragranceExplorer.BLL.Parser.Houses.Interfaces;
using FragranceExplorer.BLL.Parser.Houses.Services;
using FragranceExplorer.BLL.Parser.Perfumes.Interfaces;
using FragranceExplorer.BLL.Parser.Perfumes.Services;
using FragranceExplorer.BLL.Repositories;
using FragranceExplorer.BLL.Services;
using FragranceExplorer.BLL.Strategies;

namespace FragranceExplorer_Back;

public class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Contains("--test-parser"))
        {
            await RunParserDemoAsync();
            return;
        }

        if (args.Contains("--test-house-parser"))
        {
            await RunHouseParserDemoAsync();
            return;
        }

        var builder = WebApplication.CreateBuilder(args);

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

    private static async Task RunParserDemoAsync()
    {
        Console.WriteLine("=================================================");
        Console.WriteLine("        STARTING DATASET PARSER DEMO             ");
        Console.WriteLine("=================================================");

        // finding perfumes_actual.jsonl
        var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        string? datasetPath = null;

        while (currentDir != null)
        {
            var candidate = Path.Combine(currentDir.FullName, "FragranceExplorer.BLL", "Parser", "perfumes_actual.jsonl");
            if (File.Exists(candidate))
            {
                datasetPath = candidate;
                break;
            }
            currentDir = currentDir.Parent;
        }

        if (datasetPath == null || !File.Exists(datasetPath))
        {
            Console.WriteLine("[ERROR] Dataset file 'perfumes_actual.jsonl' was not found!");
            return;
        }

        Console.WriteLine($"[INFO] Dataset found: {datasetPath}");
        Console.WriteLine($"[INFO] File size: {new FileInfo(datasetPath).Length / (1024 * 1024)} MB");
        Console.WriteLine("[INFO] Full dataset parsing via IAsyncEnumerable...\n");

        // use a polymorphic interface IPerfumeDataSetParser
        IPerfumeDataSetParser parser = new PerfumeDataSetParser();
        var options = new ParserOptions
        {
            PathToDataset = datasetPath,
            MaxPerfumesToParse = null // read all perfumes without restrictions
        };

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        int count = 0;

        await foreach (var perfume in parser.ParseAsync(options))
        {
            count++;

            // display only the first 3 perfumes in detail to check the data structure
            if (count <= 3)
            {
                Console.WriteLine($"--- Perfume Sample #{count} ---");
                Console.WriteLine($"ID:          {perfume.Id}");
                Console.WriteLine($"Name:        {perfume.Name}");
                Console.WriteLine($"Brand:       {perfume.Brand}");
                Console.WriteLine($"Gender:      {perfume.Gender}");
                Console.WriteLine($"Rating:      {perfume.Rating:F2} / 5.00");
                Console.WriteLine($"Image URL:   {perfume.ImageUrl ?? "none"}");

                var accordVector = perfume.ToAccordsVector();
                var accordDetails = accordVector.Significances.Select(kv => $"{kv.Key} ({kv.Value:F2})");
                Console.WriteLine($"Accords:     {string.Join(", ", accordDetails)}");

                var noteVector = perfume.ToNotesVector();
                var noteDetails = noteVector.Significances.Select(kv => $"{kv.Key} ({kv.Value:F2})");
                Console.WriteLine($"Notes:       {string.Join(", ", noteDetails)}");
                Console.WriteLine();
            }
            // periodic progress status for a large dataset
            Console.WriteLine($"[PROGRESS] Processed {count:N0} perfumes... ({stopwatch.ElapsedMilliseconds} ms)");

        }

        stopwatch.Stop();
        Console.WriteLine("=================================================");
        Console.WriteLine($"[SUCCESS] Successfully parsed {count:N0} perfumes in {stopwatch.Elapsed.TotalSeconds:F2} s!");
        Console.WriteLine("=================================================");
        Console.WriteLine(parser.GetStatisticsReport());
    }

    private static async Task RunHouseParserDemoAsync()
    {
        Console.WriteLine("=================================================");
        Console.WriteLine("      STARTING HOUSE DATASET PARSER DEMO         ");
        Console.WriteLine("=================================================");

        var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        string? datasetPath = null;

        while (currentDir != null)
        {
            var candidate = Path.Combine(currentDir.FullName, "FragranceExplorer.BLL", "Parser", "house_catalog.jsonl");
            if (File.Exists(candidate))
            {
                datasetPath = candidate;
                break;
            }
            currentDir = currentDir.Parent;
        }

        if (datasetPath == null || !File.Exists(datasetPath))
        {
            Console.WriteLine("[ERROR] Dataset file 'house_catalog.jsonl' was not found!");
            return;
        }

        Console.WriteLine($"[INFO] Dataset found: {datasetPath}");
        Console.WriteLine($"[INFO] File size: {new FileInfo(datasetPath).Length / (1024.0 * 1024.0):F2} MB");
        Console.WriteLine("[INFO] Full dataset parsing via IAsyncEnumerable...\n");

        IHouseDataSetParser parser = new HouseDataSetParser();
        var options = new ParserOptions
        {
            PathToDataset = datasetPath,
            MaxPerfumesToParse = null
        };

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        int count = 0;

        await foreach (var house in parser.ParseAsync(options))
        {
            count++;

            if (count <= 3)
            {
                Console.WriteLine($"--- House Sample #{count} ---");
                Console.WriteLine($"ID:          {house.Id}");
                Console.WriteLine($"Name:        {house.Name} ({house.DisplayName})");
                Console.WriteLine($"Initials:    {house.Initials}");
                Console.WriteLine($"Location:    {house.Location}");
                Console.WriteLine($"Archive:     {house.ArchiveCount} ({house.Count})");
                Console.WriteLine($"Rating:      {house.Rating:F2} / 5.00 ({house.ReviewsLabel})");
                Console.WriteLine($"DNA Badge:   {house.DnaBadge}");

                var dnaDetails = house.OlfactoryDna.Select(d => $"{d.Family}: {d.Percentage}% ({d.Notes})");
                Console.WriteLine($"DNA:         {string.Join(" | ", dnaDetails)}");
                Console.WriteLine($"Similar IDs: {string.Join(", ", house.SimilarHouseIds)}");
                Console.WriteLine();
            }
        }

        stopwatch.Stop();
        Console.WriteLine("=================================================");
        Console.WriteLine($"[SUCCESS] Successfully parsed {count:N0} houses in {stopwatch.Elapsed.TotalSeconds:F2} s!");
        Console.WriteLine("=================================================");
        Console.WriteLine(parser.GetStatisticsReport());
    }
}


