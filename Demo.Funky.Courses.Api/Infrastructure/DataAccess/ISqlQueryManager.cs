using System.Data.SqlClient;
using LanguageExt.Common;

namespace Demo.Funky.Courses.Api.Infrastructure.DataAccess;

public interface ISqlQueryManager
{
    Aff<SqlConnection> GetConnection();
    Aff<Either<Error, TData>> GetDataItem<TQuery, TData>(SqlConnection connection, string sql, TQuery query) where TQuery : IQuery;
}