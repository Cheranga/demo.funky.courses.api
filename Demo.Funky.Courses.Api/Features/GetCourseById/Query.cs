using Demo.Funky.Courses.Api.Infrastructure.DataAccess;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public record Query (string Id) : IQuery;