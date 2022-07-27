using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using FluentValidation;
using LanguageExt.Common;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public sealed class RequestHandler : IRequestHandler<Request, Fin<string>>
{
    private readonly IValidator<Request> _validator;
    private readonly ICommandHandler<Command> _commandHandler;
    private readonly ILogger<RequestHandler> _logger;

    public RequestHandler(
        IValidator<Request> validator,
        ICommandHandler<Command> commandHandler,
        ILogger<RequestHandler> logger)
    {
        _validator = validator;
        _commandHandler = commandHandler;
        _logger = logger;
    }

    private Aff<Command> ParseRequest(Request request) =>
        AffMaybe<Command>(
            async () =>
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return Error.Many(validationResult.Errors.ToSeq().Map(ve => Error.New(ve.ErrorMessage)));
                return new Command(request.Name, request.EnrollmentDate);
            });

    private Aff<T> LogFailure<T>(Error e)
    {
        _logger.LogError(e.ToException(), ErrorMessages.DataAccessError);
        return LanguageExt.Aff<T>.Fail(e);
    }

    private Aff<string> HandleAff(Request request, CancellationToken token) => (
        from command in ParseRequest(request)
        from courseId in _commandHandler.ExecuteAsync(command)
        select courseId) | @catch(LogFailure<string>);

    public async Task<Fin<string>> Handle(Request request, CancellationToken cancellationToken) =>
        await HandleAff(request, cancellationToken).Run();
}