using System.Data.SqlClient;
using Dapper;
using Demo.Funky.Courses.Api.Extensions;
using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt.Common;

namespace Demo.Funky.Courses.Api.Infrastructure.DataAccess;

public sealed class SqlQueryManager : ISqlQueryManager
{
    private readonly DatabaseConfig _config;
    private readonly ILogger<SqlQueryManager> _logger;

    public SqlQueryManager(DatabaseConfig config, ILogger<SqlQueryManager> logger)
    {
        _config = config;
        _logger = logger;
    }

    public Aff<SqlConnection> GetConnection() =>
        AffMaybe<SqlConnection>(async () =>
        {
            var connection = new SqlConnection(_config.DatabaseConnectionString);
            await connection.OpenAsync();
            return connection;
        });

    public Aff<TData> GetDataItem<TQuery, TData>(string sql, TQuery query) where TQuery : IQuery =>
        from data in use(GetConnection(), con => con.GetData<TQuery, TData>(sql, query))
            .BiMap(
                data => data,
                error => error.ToException() switch
                {
                    ValueIsNullException => Error.New(ErrorCodes.NotFound, ErrorMessages.NotFound),
                    _ => Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError)
                }
            )
        select data;
}