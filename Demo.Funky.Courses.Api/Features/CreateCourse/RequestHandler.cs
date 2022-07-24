using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using FluentValidation;
using FluentValidation.Results;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public class RequestHandler : IRequestHandler<Request, Either<Error, string>>
{
    private readonly IValidator<Request> _validator;
    private readonly ICommandHandler<Command> _commandHandler;
    private readonly ILogger<RequestHandler> _logger;

    public RequestHandler(IValidator<Request> validator, ICommandHandler<Command> commandHandler, ILogger<RequestHandler> logger)
    {
        _validator = validator;
        _commandHandler = commandHandler;
        _logger = logger;
    }

    private async Task<Validation<ValidationFailure, Request>> Validate(Request request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (validationResult.IsValid)
        {
            return Validation<ValidationFailure, Request>.Success(request);
        }

        return Validation<ValidationFailure, Request>.Fail(Seq<ValidationFailure>(validationResult.Errors));
    }

    public async Task<Either<Error, string>> Handle(Request request, CancellationToken cancellationToken)
    {
        // TODO: is there a much better way to call these methods?
        return await (await Validate(request)).MatchAsync(
            FailAsync: seq =>
            {
                // TODO: what if there are multiple errors?
                var error = seq.Map(failure => new {ErrorCode = ErrorCodes.InvalidRequest, failure.ErrorMessage}).First();
                return Either<Error, string>.Left(Error.New(error.ErrorCode, error.ErrorMessage)).AsTask();
            },
            SuccAsync: async x =>
            {
                return (await _commandHandler.ExecuteAsync(new Command(x.Name, x.EnrollmentDate)).Run())
                    .Match(
                        Succ: id => string.IsNullOrWhiteSpace(id) ? Either<Error, string>.Left(Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError)) : Either<Error, string>.Right(id),
                        Fail: error =>
                        {
                            _logger.LogError(error.ToException(), ErrorMessages.DataAccessError);
                            return Either<Error, string>.Left(Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError, error));
                        }
                    );
            }
        );
    }
}