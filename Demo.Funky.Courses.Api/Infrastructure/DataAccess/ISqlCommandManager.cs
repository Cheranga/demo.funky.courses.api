using System.Data.SqlClient;
using Dapper;
using Demo.Funky.Courses.Api.Extensions;
using Demo.Funky.Courses.Api.Features.Shared;
using LanguageExt.Common;

namespace Demo.Funky.Courses.Api.Infrastructure.DataAccess;

public interface ISqlCommandManager
{
    Aff<TData> ExecuteAsync<TData, TCommand>(string sql, TCommand command) where TCommand : ICommand;
}

public sealed class SqlCommandManager : ISqlCommandManager
{
    private readonly DatabaseConfig _config;

    public SqlCommandManager(DatabaseConfig config)
    {
        _config = config;
    }
    private static Aff<TData> ExecuteCommandAsync<TCommand, TData>(SqlConnection connection, string sql, TCommand command) =>
        connection.QueryFirstOrDefaultAsync<TData>(sql, command).ToAff();

    public Aff<TData> ExecuteAsync<TData, TCommand>(string sql, TCommand command) where TCommand : ICommand=>
        from data in _config.DatabaseConnectionString.UseOpenConnection(con => ExecuteCommandAsync<TCommand, TData>(con, sql, command))
            .BiMap(
                data => data,
                error => error.ToException() switch
                {
                    ValueIsNullException => Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError),
                    _ => Error.New(ErrorCodes.DataAccessError, ErrorMessages.DataAccessError)
                })
        select data;
    
}