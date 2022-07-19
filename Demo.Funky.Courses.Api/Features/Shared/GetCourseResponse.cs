using LanguageExt;

namespace Demo.Funky.Courses.Api.Features.Shared;

public class GetCourseResponse
{
    public string Id { get; }
    public string Name { get; }

    public GetCourseResponse(string id, string name)
    {
        Id = id;
        Name = name;
    }
}

public class ErrorResponse
{
    public int ErrorCode { get; }
    public Lst<string> Errors { get; }

    public ErrorResponse(int errorCode, Lst<string> errors)
    {
        ErrorCode = errorCode;
        Errors = errors;
    }
}