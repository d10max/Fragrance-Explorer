using FragranceExplorer.DAL.Context;
using FragranceExplorer.DAL.Repositories.Interfaces;
using FragranceExplorer.DAL.Repositories.Realizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FragranceExplorer.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<FragranceDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly(typeof(FragranceDbContext).Assembly.FullName)));

        services.AddScoped<IPerfumeRepository, PerfumeRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();

        return services;
    }
}
