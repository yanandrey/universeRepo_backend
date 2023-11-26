using Microsoft.EntityFrameworkCore;
using UniverseRepo.Infra.Context;

namespace UniverseRepo.Configurations;

public static class DbContextServiceCollection
{
    public static void AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:DatabaseConnection"];

        services.AddDbContext<ApiDbContext>(options =>
        {
            options.UseSqlServer(connectionString, x =>
            {
                x.EnableRetryOnFailure(1, TimeSpan.FromSeconds(1), null);
            });

            //options.UseInMemoryDatabase("UniverseRepo");
        });
    }
}