using System.Data.SqlClient;
using Dapper;
using Demo.Funky.Courses.Api.Features.Shared;
using Demo.Funky.Courses.Api.Infrastructure.DataAccess;
using LanguageExt.Common;
using LanguageExt.SomeHelp;

namespace Demo.Funky.Courses.Api.Extensions;

public static class SqlConnectionExt
{
    private static Aff<SqlConnection> GetConnection(string connectionString) =>
        AffMaybe<SqlConnection>(
            async () =>
            {
                var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return connection;
            });

    public static Aff<T> UseOpenConnection<T>(this string connectionString, Func<SqlConnection, Aff<T>> usingFn) =>
        use(GetConnection(connectionString), usingFn);

    public static Aff<Option<TReturn>> QueryFirstOrNone<TReturn, TInput>(
        this SqlConnection connection,
        string command,
        TInput input) =>
        AffMaybe<Option<TReturn>>(async () => Optional(await connection.QueryFirstOrDefaultAsync<TReturn>(command, input)));

    public static Aff<TData> GetData<TQuery, TData>(this SqlConnection connection, string sql, TQuery query) where TQuery : IQuery =>
        AffMaybe<TData>(async () => await connection.QueryFirstOrDefaultAsync<TData>(sql, query));
}