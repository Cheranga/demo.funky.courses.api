using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt;
using LanguageExt.Common;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.GetCourseByName;

public class Request : IRequest<Either<Error, GetCourseResponse>>
{
    public string Name { get; }

    public Request(string name)
    {
        Name = name;
    }
}