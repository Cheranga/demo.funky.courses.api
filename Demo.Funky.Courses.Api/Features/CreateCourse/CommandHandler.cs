using System.Data.SqlClient;
using Dapper;
using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public class CommandHandler : ICommandHandler<Command>
{
    private readonly DatabaseConfig _config;

    public CommandHandler(DatabaseConfig config)
    {
        _config = config;
    }

    private static string CommandText => @"insert into tblCourses (name, enrollmentdate) " +
                                         "output inserted.Id, inserted.Name, inserted.EnrollmentDate " +
                                         "values (@Name, @EnrollmentDate)";

    private Eff<SqlConnection> GetConnection(string connectionString) => EffMaybe<SqlConnection>(() => new SqlConnection(connectionString));

    // is there a better way to write this method?
    public Aff<string> ExecuteAsync(Command command) =>
        use(GetConnection(_config.DatabaseConnectionString), connection => AffMaybe<string>(async () =>
        {
            await connection.OpenAsync();
            var dataModel = await connection.QueryFirstOrDefaultAsync<CourseDataModel>(CommandText, command);
            if (string.IsNullOrWhiteSpace(dataModel?.Id))
            {
                return Error.New(ErrorCodes.CreateCourseDataAccessError, ErrorMessages.CreateCourseDataAccessError);
            }

            return dataModel.Id;
        }));
}