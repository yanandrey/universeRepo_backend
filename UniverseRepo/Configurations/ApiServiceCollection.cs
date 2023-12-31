﻿namespace UniverseRepo.Configurations;

public static class ApiServiceCollection
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextServices(configuration);
        services.AddScopedServices();
        services.AddSwaggerServices();
        services.AddTokenServices(configuration);
    }
}