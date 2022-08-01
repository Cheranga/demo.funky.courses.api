using Demo.Funky.Courses.Api.Extensions;
using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt.Common;

namespace Demo.Funky.Courses.Api.Features.GetCourseByName;

public sealed class QueryHandler : IQueryHandler<Query, CourseDataModel>
{
    private readonly DatabaseConfig _config;

    public QueryHandler(DatabaseConfig config) =>
        _config = config;

    private static string SqlStatement =>
        @"select * from tblCourses where Name=@Name";

    public Aff<CourseDataModel> GetAsync(Query query) =>
        GetCourseAsync(query);

    private Aff<CourseDataModel> GetCourseAsync(Query query) =>
        _config.DatabaseConnectionString.UseOpenConnection(
            connection =>
                from optionalCourseData in connection.QueryFirstOrNone<CourseDataModel, Query>(SqlStatement, query)
                from courseData in optionalCourseData.ToAff(Error.New(ErrorCodes.CourseNotFound, ErrorMessages.CourseNotFound))
                select courseData);
}