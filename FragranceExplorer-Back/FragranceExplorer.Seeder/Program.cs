using FragranceExplorer.BLL.Parser.Houses.Interfaces;
using FragranceExplorer.BLL.Parser.Houses.Services;
using FragranceExplorer.BLL.Parser.Perfumes.Interfaces;
using FragranceExplorer.BLL.Parser.Perfumes.Services;
using FragranceExplorer.DAL;
using FragranceExplorer.Seeder.Configuration;
using FragranceExplorer.Seeder.Interfaces;
using FragranceExplorer.Seeder.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FragranceExplorer.Seeder;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory
        });

        builder.Configuration.AddUserSecrets<Program>(optional: true);
        builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

        builder.Services.Configure<SeederOptions>(
            builder.Configuration.GetSection(SeederOptions.SectionName));

        var cliOptions = SeederCliOptions.Parse(args);
        builder.Services.AddSingleton(cliOptions);

        builder.Services.AddDal(builder.Configuration);

        builder.Services.AddTransient<IHouseDataSetParser, HouseDataSetParser>();
        builder.Services.AddTransient<IPerfumeDataSetParser, PerfumeDataSetParser>();

        builder.Services.AddScoped<IHouseSeeder, HouseSeeder>();
        builder.Services.AddScoped<IPerfumeSeeder, PerfumeSeeder>();
        builder.Services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

        using var host = builder.Build();

        using var scope = host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();

        logger.LogInformation(
            "Starting FragranceExplorer Seeder (Reseed={Reseed}, HousesOnly={HousesOnly}, PerfumesOnly={PerfumesOnly})",
            cliOptions.Reseed,
            cliOptions.HousesOnly,
            cliOptions.PerfumesOnly);

        await seeder.SeedAsync();
    }
}
