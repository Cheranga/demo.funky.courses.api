using System.Net;
using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Funky.Courses.Api.Features.GetCourseByName;

public static class ResponseHandler
{
    public static IActionResult ToActionResult(this Fin<GetCourseResponse> operation) =>
        operation.Match(response => new OkObjectResult(response), GetErrorResponse);

    private static IActionResult GetErrorResponse(Error error) =>
        // TODO - we can handle multiple errors here as well
        error.Code switch
        {
            ErrorCodes.CourseNotFound => new ObjectResult(new ErrorResponse(error.Code, Seq1(error.Message)))
                { StatusCode = (int)(HttpStatusCode.NotFound) },
            _ => new ObjectResult(new ErrorResponse(error.Code, Seq1(error.Message)))
                { StatusCode = (int)(HttpStatusCode.InternalServerError) }
        };
}