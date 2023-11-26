using UniverseRepo.Core.Helpers;
using UniverseRepo.Core.Repositories;
using UniverseRepo.Core.Services;

namespace UniverseRepo.Configurations;

public static class ScopedServiceCollection
{
    public static void AddScopedServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenHelper, TokenHelper>();
        services.AddScoped<IRepositoryRepository, RepositoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
    }
}