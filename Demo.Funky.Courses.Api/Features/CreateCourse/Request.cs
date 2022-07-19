using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt;
using LanguageExt.Common;
using MediatR;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public class Request : IRequest<Either<Error, string>>
{
    public string Name { get; }
    public DateTime EnrollmentDate { get; }

    public Request(string name, DateTime enrollmentDate)
    {
        Name = name;
        EnrollmentDate = enrollmentDate;
    }
}