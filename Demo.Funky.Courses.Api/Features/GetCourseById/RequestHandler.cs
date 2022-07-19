using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt;
using LanguageExt.Common;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public class RequestHandler : IRequestHandler<Request, Either<Error, GetCourseResponse>>
{
    private readonly IQueryHandler<Query, CourseDataModel> _queryHandler;
    private readonly ILogger<RequestHandler> _logger;

    public RequestHandler(IQueryHandler<Query, CourseDataModel> queryHandler, ILogger<RequestHandler> logger)
    {
        _queryHandler = queryHandler;
        _logger = logger;
    }

    public async Task<Either<Error, GetCourseResponse>> Handle(Request request, CancellationToken cancellationToken)
    {
        return (await _queryHandler.GetAsync(new Query(request.Id)).Run())
            .Match(
                model => string.IsNullOrWhiteSpace(model.Id) ?
                    Either<Error, GetCourseResponse>.Left(Error.New(ErrorCodes.CourseNotFound, ErrorMessages.CourseNotFound)) : 
                    Either<Error, GetCourseResponse>.Right(new GetCourseResponse(model.Id, model.Name)),
                error =>
                {
                    _logger.LogError(error.ToException(), ErrorMessages.DataAccessError);
                    return Either<Error, GetCourseResponse>.Left(Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError, error));
                });
    }
}