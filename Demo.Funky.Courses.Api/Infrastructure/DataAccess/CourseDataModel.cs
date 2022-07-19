using LanguageExt;

namespace Demo.Funky.Courses.Api.Infrastructure.DataAccess;

public class CourseDataModel
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public DateTime EnrollmentDate { get; set; }
}