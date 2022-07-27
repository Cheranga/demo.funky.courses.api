namespace Demo.Funky.Courses.Api.Features.Shared;

public sealed record GetCourseResponse(string Id, string Name);

public sealed record ErrorResponse(int ErrorCode, Seq<string> Errors);