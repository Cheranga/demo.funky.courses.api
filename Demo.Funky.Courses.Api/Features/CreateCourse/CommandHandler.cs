using System.Data.SqlClient;
using Demo.Funky.Courses.Api.Extensions;
using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt.Common;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public sealed class CommandHandler : ICommandHandler<Command>
{
    private readonly DatabaseConfig _config;

    public CommandHandler(DatabaseConfig config) =>
        _config = config;

    private static string CommandText =>
        @"insert into tblCourses (name, enrollmentdate) " +
        "output inserted.Id, inserted.Name, inserted.EnrollmentDate " +
        "values (@Name, @EnrollmentDate)";

    private static Aff<string> FindCourseId(SqlConnection connection, Command command) =>
        from optionalCourse in connection.QueryFirstOrNone<CourseDataModel, Command>(CommandText, command)
        from courseData in optionalCourse.ToAff(Error.New(ErrorCodes.CourseNotFound, "Failed to find course"))
        from _1 in guard(
            string.IsNullOrWhiteSpace(courseData.Id),
            Error.New(ErrorCodes.CreateCourseDataAccessError, ErrorMessages.CreateCourseDataAccessError))
        select courseData.Id;

    public Aff<string> ExecuteAsync(Command command) =>
        _config.DatabaseConnectionString.UseOpenConnection(connection => FindCourseId(connection, command));
}