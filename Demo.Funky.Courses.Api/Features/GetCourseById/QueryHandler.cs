using System.Data.SqlClient;
using Dapper;
using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public class QueryHandler : IQueryHandler<Query, CourseDataModel>
{
    private readonly DatabaseConfig _config;

    public QueryHandler(DatabaseConfig config)
    {
        _config = config;
    }

    private static string CommandText => @"select * from tblCourses where id=@id";

    private Eff<SqlConnection> GetConnection(string connectionString) => EffMaybe<SqlConnection>(() => new SqlConnection(connectionString));

    public Aff<CourseDataModel> GetAsync(Query query) => GetCourseAsync(query);

    // is there a better way to write this method?
    private Aff<CourseDataModel> GetCourseAsync(Query query)=>
        use(GetConnection(_config.DatabaseConnectionString), connection => AffMaybe<CourseDataModel>(async () =>
        {
            await connection.OpenAsync();
            var dataModel = await connection.QueryFirstOrDefaultAsync<CourseDataModel>(CommandText, query);
            if (string.IsNullOrWhiteSpace(dataModel?.Id))
            {
                return Error.New(ErrorCodes.CourseNotFound, ErrorMessages.CourseNotFound);
            }

            return dataModel;
        }));
}