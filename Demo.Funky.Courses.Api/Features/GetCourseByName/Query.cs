using Demo.Funky.Courses.Api.Infrastructure.DataAccess;

namespace Demo.Funky.Courses.Api.Features.GetCourseByName;

public sealed record Query(string Name) : IQuery;