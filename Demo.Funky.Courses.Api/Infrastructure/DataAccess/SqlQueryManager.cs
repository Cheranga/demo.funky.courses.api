using System.Data.SqlClient;
using Dapper;
using Demo.Funky.Courses.Api.Extensions;
using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt.Common;

namespace Demo.Funky.Courses.Api.Infrastructure.DataAccess;

public sealed class SqlQueryManager : ISqlQueryManager
{
    private readonly DatabaseConfig _config;

    public SqlQueryManager(DatabaseConfig config)
    {
        _config = config;
    }

    private static Aff<TData> GetSingleItem<TQuery, TData>(SqlConnection connection, string sql, TQuery query) =>
        connection.QueryFirstOrDefaultAsync<TData>(sql, query).ToAff();

    public Aff<TData> GetDataItem<TQuery, TData>(string sql, TQuery query) where TQuery : IQuery =>
        from data in _config.DatabaseConnectionString.UseOpenConnection(con => GetSingleItem<TQuery, TData>(con, sql, query))
            .BiMap(
                data => data,
                error => error.ToException() switch
                {
                    ValueIsNullException => Error.New(ErrorCodes.NotFound, ErrorMessages.NotFound),
                    _ => Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError)
                })
        select data;
}