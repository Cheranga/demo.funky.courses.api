using Demo.Funky.Courses.Api.Features.Shared;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public sealed record Request(string Id) : IRequest<Fin<GetCourseResponse>>;