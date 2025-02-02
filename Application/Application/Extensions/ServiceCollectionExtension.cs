using Application.Application.Admins;
using Application.Application.Users;
using Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<IUserService, UserService>();
        collection.AddScoped<IAdminService, AdminService>();

        return collection;
    }
}