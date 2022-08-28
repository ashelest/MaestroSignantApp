using MaestroSignant.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaestroSignant.Application;

public static class ApplicationBootstrap
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostingsServiceConfig>(configuration.GetSection(PostingsServiceConfig.Position));
        services.Configure<PostingAdmin>(configuration.GetSection(PostingAdmin.Position));

        return services;
    }
}