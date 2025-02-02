using Abstractions.Repository;
using Application.Repositories;
using Itmo.Dev.Platform.Postgres.Extensions;
using Itmo.Dev.Platform.Postgres.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Abstractions.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection collection,
        Action<PostgresConnectionConfiguration> configuration)
    {
        collection.AddPlatformPostgres(builder => builder.Configure(configuration));

        collection.AddScoped<IUserRepository, UserRepository>();
        collection.AddScoped<IAdminRepository, AdminRepository>();

        return collection;
    }
}