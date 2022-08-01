using System.Data.SqlClient;
using Dapper;
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

    private Aff<Either<Error, TData>> GetSingleItem<TQuery, TData>(SqlConnection connection, string sql, TQuery query) where TQuery : IQuery =>
        AffMaybe<Either<Error, TData>>(async () =>
        {
            // TODO: make the error codes and messages generic
            return await TryAsync(async () => await connection.QueryFirstOrDefaultAsync<TData>(sql, query))
                .Match(
                    model => model == null ? Either<Error, TData>.Left(Error.New(ErrorCodes.CourseNotFound, ErrorMessages.CourseNotFound)) : Either<Error, TData>.Right(model),
                    exception =>
                    {
                        _logger.LogError(exception, "error occurred when accessing data");
                        return Either<Error, TData>.Left(Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError, exception));
                    });
        });

    public Aff<Either<Error, TData>> GetDataItem<TQuery, TData>(string sql, TQuery query) where TQuery : IQuery =>
        from operation in use(GetConnection(), con => GetSingleItem<TQuery, TData>(con, sql, query))
        select operation;
}