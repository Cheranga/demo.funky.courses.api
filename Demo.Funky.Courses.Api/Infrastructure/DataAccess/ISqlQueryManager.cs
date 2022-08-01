using System.Data.SqlClient;
using LanguageExt.Common;

namespace Demo.Funky.Courses.Api.Infrastructure.DataAccess;

public interface ISqlQueryManager
{
    Aff<Either<Error, TData>> GetDataItem<TQuery, TData>(string sql, TQuery query) where TQuery : IQuery;
    // TODO: implement a method to return multiple results as well.
}