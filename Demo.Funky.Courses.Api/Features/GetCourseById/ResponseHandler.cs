using System.Net;
using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

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
            _ => 
                error.Code < 0 ? new ObjectResult(new ErrorResponse(ErrorCodes.DataAccessError, toSeq(new[] { ErrorMessages.DataAccessError }))) { StatusCode = (int)(HttpStatusCode.InternalServerError) } :
                new ObjectResult(new ErrorResponse(error.Code, toSeq(new[] { error.Message }))) { StatusCode = (int)(HttpStatusCode.InternalServerError) }
        };
}