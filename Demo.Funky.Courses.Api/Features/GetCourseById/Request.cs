using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt;
using LanguageExt.Common;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public class Request : IRequest<Either<Error, GetCourseResponse>>
{
    public string Id { get; }

    public Request(string id)
    {
        Id = id;
    }
}