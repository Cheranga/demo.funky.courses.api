using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt;
using LanguageExt.Common;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.GetCourseByName;

public class RequestHandler : IRequestHandler<Request, Either<Error, GetCourseResponse>>
{
    public Task<Either<Error, GetCourseResponse>> Handle(Request request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}