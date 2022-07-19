using Demo.Funky.Courses.Api.Features.GetCourseById;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using MediatR;

namespace Demo.Funky.Courses.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void RegisterDependencies(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddMediatR(typeof(WebApplicationBuilderExtensions).Assembly);

        RegisterDataAccess(services);
    }

    private static void RegisterDataAccess(IServiceCollection services)
    {
        services.AddSingleton<IQueryHandler<Query, CourseDataModel>, QueryHandler>();
    }
}