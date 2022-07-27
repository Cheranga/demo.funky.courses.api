using Demo.Funky.Courses.Api.Infrastructure.DataAccess;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public sealed record Command(string Name, DateTime EnrollmentDate) : ICommand;