using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

[ApiController]
public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("api/courses/id/{courseId}")]
    public async Task<IActionResult> GetCourseByIdAsync([FromRoute] string courseId)
    {
        return (await _mediator.Send(new Request(courseId))).ToActionResult();
    }
}