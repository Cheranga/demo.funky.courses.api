using System.Net;
using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public static class ResponseHandler
{
    public static IActionResult ToActionResult(this Either<Error, GetCourseResponse> operation)
    {
        return operation.Match<IActionResult>(
            Left: error => new ObjectResult(error)
            {
                StatusCode = (int) HttpStatusCode.InternalServerError
            },
            Right: response => new OkObjectResult(response)
        );
    }
}