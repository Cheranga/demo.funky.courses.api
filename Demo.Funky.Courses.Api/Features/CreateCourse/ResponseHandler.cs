using System.Net;
using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public static class ResponseHandler
{
    public static IActionResult ToActionResult(this Fin<string> operation) =>
        operation.Match(
            response => new CreatedAtRouteResult(
                nameof(GetCourseById.Controller.GetCourseById),
                new { courseId = response },
                value: null),
            GetErrorResponse);

    private static IActionResult GetErrorResponse(Error error) =>
        // TODO - we can handle multiple errors here as well
        error.Code switch
        {
            ErrorCodes.NotFound => new ObjectResult(new ErrorResponse(error.Code, Seq1(error.Message)))
                { StatusCode = (int)(HttpStatusCode.NotFound) },
            _ => new ObjectResult(new ErrorResponse(error.Code, Seq1(error.Message)))
                { StatusCode = (int)(HttpStatusCode.InternalServerError) }
        };
}