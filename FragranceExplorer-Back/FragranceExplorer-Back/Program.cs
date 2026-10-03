using FragranceExplorer.BLL.DataSetParser.Common;
using FragranceExplorer.BLL.DataSetParser.Interfaces;
using FragranceExplorer.BLL.DataSetParser.Services;
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

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IPerfumeDataSetParser, PerfumeDataSetParser>();

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

            Console.WriteLine("Починаємо завантаження датасету в пам'ять.");

            await repository.InitializeAsync();

            Console.WriteLine("Датасет успішно завантажено.");
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
        Console.WriteLine("        ЗАПУСК ДЕМОНСТРАЦІЇ DATASET PARSER       ");
        Console.WriteLine("=================================================");

        // Знаходимо perfumes_actual.jsonl
        var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        string? datasetPath = null;

        while (currentDir != null)
        {
            var candidate = Path.Combine(currentDir.FullName, "FragranceExplorer.BLL", "DataSetParser", "perfumes_actual.jsonl");
            if (File.Exists(candidate))
            {
                datasetPath = candidate;
                break;
            }
            currentDir = currentDir.Parent;
        }

        if (datasetPath == null || !File.Exists(datasetPath))
        {
            Console.WriteLine("[ПОМИЛКА] Файл датасету 'perfumes_actual.jsonl' не знайдено!");
            return;
        }

        Console.WriteLine($"[INFO] Датасет знайдено: {datasetPath}");
        Console.WriteLine($"[INFO] Розмір файлу: {new FileInfo(datasetPath).Length / (1024 * 1024)} MB");
        Console.WriteLine("[INFO] Повний парсинг датасету через IAsyncEnumerable...\n");

        // Використовуємо поліморфний інтерфейс IPerfumeDataSetParser
        IPerfumeDataSetParser parser = new PerfumeDataSetParser();
        var options = new ParserOptions
        {
            PathToDataset = datasetPath,
            MaxPerfumesToParse = null // Зчитуємо всі парфуми без обмежень
        };

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        int count = 0;

        await foreach (var perfume in parser.ParseAsync(options))
        {
            count++;

            // Детально виводимо лише перші 3 парфуми для перевірки структури даних
            if (count <= 3)
            {
                Console.WriteLine($"--- Зразок парфуму #{count} ---");
                Console.WriteLine($"ID:          {perfume.Id}");
                Console.WriteLine($"Назва:       {perfume.Name}");
                Console.WriteLine($"Бренд:       {perfume.Brand}");
                Console.WriteLine($"Категорія:   {perfume.Gender}");
                Console.WriteLine($"Рейтинг:     {perfume.Rating:F2} / 5.00");
                Console.WriteLine($"Зображення:  {perfume.ImageUrl ?? "немає"}");

                var accordVector = perfume.ToAccordsVector();
                var accordDetails = accordVector.Significances.Select(kv => $"{kv.Key} ({kv.Value:F2})");
                Console.WriteLine($"Акорди:      {string.Join(", ", accordDetails)}");

                var noteVector = perfume.ToNotesVector();
                var noteDetails = noteVector.Significances.Select(kv => $"{kv.Key} ({kv.Value:F2})");
                Console.WriteLine($"Ноти:      {string.Join(", ", noteDetails)}");
                Console.WriteLine();
            }
            // Періодичний статус прогресу для великого датасету
            Console.WriteLine($"[ПРОГРЕС] Оброблено {count:N0} парфумів... ({stopwatch.ElapsedMilliseconds} мс)");

        }

        stopwatch.Stop();
        Console.WriteLine("=================================================");
        Console.WriteLine($"[УСПІХ] Успішно розпарсено {count:N0} парфумів за {stopwatch.Elapsed.TotalSeconds:F2} с!");
        Console.WriteLine("=================================================");
        Console.WriteLine(parser.GetStatisticsReport());
    }
}


