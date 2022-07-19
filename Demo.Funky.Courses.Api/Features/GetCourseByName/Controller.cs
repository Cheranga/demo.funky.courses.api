using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Funky.Courses.Api.Features.GetCourseByName;

[ApiController]
public class Controller : ControllerBase
{
    private readonly IMediator _mediator;
    
    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("api/courses/name/{courseName}")]
    public async Task<IActionResult> GetAsync([FromRoute] string courseName)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return Ok();
    }
}