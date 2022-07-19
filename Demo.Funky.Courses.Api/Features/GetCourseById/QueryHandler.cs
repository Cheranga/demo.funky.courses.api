using System.Data.SqlClient;
using Dapper;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt;
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


    public Aff<CourseDataModel> GetAsync(Query query) => GetCourseAsync(query);

    // is there a better way to write this method?
    private Aff<CourseDataModel> GetCourseAsync(Query query)=>
        TryAsync(async () =>
        {
            using (var connection = new SqlConnection(_config.DatabaseConnectionString))
            {
                await connection.OpenAsync();
                var dataModel = await connection.QueryFirstOrDefaultAsync<CourseDataModel>(CommandText, query);
                return string.IsNullOrWhiteSpace(dataModel?.Id) ? new CourseDataModel() : dataModel;
            }
        }).ToAff();
}