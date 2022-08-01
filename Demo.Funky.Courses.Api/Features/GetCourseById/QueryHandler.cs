using Demo.Funky.Courses.Api.Infrastructure.DataAccess;

namespace Demo.Funky.Courses.Api.Features.GetCourseById;

public sealed class QueryHandler : IQueryHandler<Query, CourseDataModel>
{
    private readonly ISqlQueryManager _queryManager;

    public QueryHandler(ISqlQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    private static string SqlStatement =>
        @"select * from tblCourses where id=@id";

    public Aff<CourseDataModel> GetAsync(Query query) =>
        from getCourseOperation in use(_queryManager.GetConnection(), connection => _queryManager.GetDataItem<Query, CourseDataModel>(connection, SqlStatement, query))
        from course in getCourseOperation.ToAff()
        select course;
}