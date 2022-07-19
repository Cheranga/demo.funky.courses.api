﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

[ApiController]
public class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("api/courses")]
    public async Task<IActionResult> GetCourseByNameAsync([FromBody]Request request)
    {
        return (await _mediator.Send(request)).ToActionResult();
    }
}