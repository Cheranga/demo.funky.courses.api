using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt;
using LanguageExt.Common;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public class RequestHandler : IRequestHandler<Request, Either<Error, string>>
{
    private readonly ICommandHandler<Command> _commandHandler;
    private readonly ILogger<RequestHandler> _logger;

    public RequestHandler(ICommandHandler<Command> commandHandler, ILogger<RequestHandler> logger)
    {
        _commandHandler = commandHandler;
        _logger = logger;
    }

    public async Task<Either<Error, string>> Handle(Request request, CancellationToken cancellationToken)
    {
        return (await _commandHandler.ExecuteAsync(new Command(request.Name, request.EnrollmentDate)).Run())
            .Match(
                id => string.IsNullOrWhiteSpace(id) ?
                    Either<Error, string>.Left(Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError)) : 
                    Either<Error, string>.Right(id),
                error =>
                {
                    _logger.LogError(error.ToException(), ErrorMessages.DataAccessError);
                    return Either<Error, string>.Left(Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError, error));
                });
    }
}