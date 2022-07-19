using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public class QueryHandler : IQueryHandler<Query, CourseDataModel>
{
    public Aff<CourseDataModel> GetAsync(Query query)
    {
        return TryOptionAsync(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            return new CourseDataModel
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = "C# Functional Programming"
            };
        }).ToAff();
    }
}