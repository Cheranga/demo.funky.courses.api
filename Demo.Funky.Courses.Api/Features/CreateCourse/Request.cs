using MediatR;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public sealed record Request(string Name, DateTime EnrollmentDate) : IRequest<Fin<string>>;