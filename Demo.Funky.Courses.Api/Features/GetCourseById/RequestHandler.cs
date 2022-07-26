using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public sealed class RequestHandler : IRequestHandler<Request, Fin<GetCourseResponse>>
{
    private readonly IQueryHandler<Query, CourseDataModel> _queryHandler;
    private readonly ILogger<RequestHandler> _logger;

    public RequestHandler(IQueryHandler<Query, CourseDataModel> queryHandler, ILogger<RequestHandler> logger)
    {
        _queryHandler = queryHandler;
        _logger = logger;
    }

    public async Task<Fin<GetCourseResponse>> Handle(Request request, CancellationToken cancellationToken) =>
        await _queryHandler.GetAsync(new Query(request.Id))
            .BiMap(
                model => new GetCourseResponse(model.Id, model.Name),
                error =>
                {
                    _logger.LogError(error.ToException(), ErrorMessages.DataAccessError);
                    return error;
                })
            .Run();
}