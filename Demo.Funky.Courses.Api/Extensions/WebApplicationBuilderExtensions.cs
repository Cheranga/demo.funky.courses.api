using Demo.Funky.Courses.Api.Features.GetCourseById;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using MediatR;

namespace Demo.Funky.Courses.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        services.AddMediatR(typeof(WebApplicationBuilderExtensions).Assembly);

        RegisterConfigurations(services, configuration);
        RegisterDataAccess(services);
    }

    private static void RegisterConfigurations(IServiceCollection services, ConfigurationManager configuration)
    {
        var databaseConfig = configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>();
        services.AddSingleton<DatabaseConfig>(databaseConfig);
    }

    private static void RegisterDataAccess(IServiceCollection services)
    {
        services.AddSingleton<IQueryHandler<Query, CourseDataModel>, QueryHandler>();
    }
}