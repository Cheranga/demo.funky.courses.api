using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Funky.Courses.Api.Features.GetCourseByName;

[ApiController]
public sealed class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator) =>
        _mediator = mediator;

    [HttpGet("api/courses/name/{courseName}", Name = nameof(GetCourseByName))]
    public async Task<IActionResult> GetCourseByName([FromRoute] string courseName) =>
        (await _mediator.Send(new Request(courseName))).ToActionResult();
}