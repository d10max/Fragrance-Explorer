
using FragranceExplorer.BLL.DataSetParser.Common;
using FragranceExplorer.BLL.DataSetParser.Interfaces;
using FragranceExplorer.BLL.DataSetParser.Services;

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

        var app = builder.Build();

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

                var accordVector = perfume.ToAccordVector();
                var accordDetails = accordVector.Significances.Select(kv => $"{kv.Key} ({kv.Value:F2})");
                Console.WriteLine($"Акорди:      {string.Join(", ", accordDetails)}");

                var noteWeights = perfume.ToNoteWeights();
                Console.WriteLine($"Кількість нот: {noteWeights.Count}");
                var topNotes = noteWeights.Take(5).Select(kv => $"{kv.Key} ({kv.Value:F2})");
                Console.WriteLine($"Зразок нот:  {string.Join(", ", topNotes)}");
                Console.WriteLine();
            }
            // Періодичний статус прогресу для великого датасету
            else if (count % 10_000 == 0)
            {
                Console.WriteLine($"[ПРОГРЕС] Оброблено {count:N0} парфумів... ({stopwatch.ElapsedMilliseconds} мс)");
            }
        }

        stopwatch.Stop();
        Console.WriteLine("=================================================");
        Console.WriteLine($"[УСПІХ] Успішно розпарсено {count:N0} парфумів за {stopwatch.Elapsed.TotalSeconds:F2} с!");
        Console.WriteLine("=================================================");
        Console.WriteLine(parser.GetStatisticsReport());
    }
}


