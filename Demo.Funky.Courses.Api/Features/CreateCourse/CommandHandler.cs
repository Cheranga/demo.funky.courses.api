using System.Data.SqlClient;
using Dapper;
using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt;
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


    // is there a better way to write this method?
    public Aff<string> ExecuteAsync(Command command) =>
        TryAsync(async () =>
        {
            using (var connection = new SqlConnection(_config.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                var dataModel = await connection.QueryFirstOrDefaultAsync<CourseDataModel>(CommandText, command);
                return string.IsNullOrWhiteSpace(dataModel?.Id) ? ErrorCodes.CommandError.ToString() : dataModel.Id;
            }
        }).ToAff();
}