﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

[ApiController]
public sealed class Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public Controller(IMediator mediator) =>
        _mediator = mediator;

    [HttpGet("api/courses/id/{courseId}", Name = nameof(GetCourseById))]
    public async Task<IActionResult> GetCourseById([FromRoute] string courseId) =>
        (await _mediator.Send(new Request(courseId))).ToActionResult();
}