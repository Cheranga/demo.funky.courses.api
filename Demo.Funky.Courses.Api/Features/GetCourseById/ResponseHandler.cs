using System.Net;
using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using static LanguageExt.Prelude;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public static class ResponseHandler
{
    public static IActionResult ToActionResult(this Either<Error, GetCourseResponse> operation)
    {
        return operation.Match(
            Left: GetErrorResponse,
            Right: response => new OkObjectResult(response)
        );
    }

    private static IActionResult GetErrorResponse(Error error) =>
        error.Code switch
        {
            ErrorCodes.CourseNotFound => new ObjectResult(new ErrorResponse(error.Code, List(error.Message))) {StatusCode = (int) (HttpStatusCode.NotFound)},
            _ => new ObjectResult(new ErrorResponse(error.Code, List(error.Message))) {StatusCode = (int) (HttpStatusCode.InternalServerError)}
        };
}