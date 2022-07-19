using Demo.Funky.Courses.Api.Infrastructure.DataAccess;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public class Query : IQuery
{
    public string Id { get; }

    public Query(string id)
    {
        Id = id;
    }
}