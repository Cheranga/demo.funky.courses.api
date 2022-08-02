using Demo.Funky.Courses.Api.Extensions;
using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt.Common;

namespace Demo.Funky.Courses.Api.Features.GetCourseByName;

public sealed class QueryHandler : IQueryHandler<Query, CourseDataModel>
{
    private readonly ISqlQueryManager _queryManager;

    public QueryHandler(ISqlQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    private static string SqlStatement =>
        @"select * from tblCourses where Name=@name";

    public Aff<CourseDataModel> GetAsync(Query query) => 
        _queryManager.GetDataItem<Query, CourseDataModel>(SqlStatement, query);
}