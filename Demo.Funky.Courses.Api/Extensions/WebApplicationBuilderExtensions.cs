using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using FluentValidation;
using MediatR;
using CourseFeatures = Demo.Funky.Courses.Api.Features;

namespace Demo.Funky.Courses.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddMediatR(typeof(WebApplicationBuilderExtensions).Assembly);
        services.AddValidatorsFromAssembly(typeof(WebApplicationBuilderExtensions).Assembly);

        RegisterConfigurations(services, configuration);
        RegisterDataAccess(services);
    }

    private static void RegisterConfigurations(IServiceCollection services, ConfigurationManager configuration)
    {
        var databaseConfig = configuration.GetSection(nameof(DatabaseConfig)).Get<DatabaseConfig>();
        services.AddSingleton(databaseConfig);
    }

    private static void RegisterDataAccess(IServiceCollection services)
    {
        services.AddSingleton<ISqlQueryManager, SqlQueryManager>();
        services.AddSingleton<ISqlCommandManager, SqlCommandManager>();
        services.AddSingleton<IQueryHandler<CourseFeatures.GetCourseById.Query, CourseDataModel>, CourseFeatures.GetCourseById.QueryHandler>();
        services.AddSingleton<IQueryHandler<CourseFeatures.GetCourseByName.Query, CourseDataModel>, CourseFeatures.GetCourseByName.QueryHandler>();
        services.AddSingleton<ICommandHandler<CourseFeatures.CreateCourse.Command>, CourseFeatures.CreateCourse.CommandHandler>();
    }
}