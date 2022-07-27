using Demo.Funky.Courses.Api.Features.Shared;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.GetCourseByName;

public sealed record Request(string Name) : IRequest<Fin<GetCourseResponse>>;