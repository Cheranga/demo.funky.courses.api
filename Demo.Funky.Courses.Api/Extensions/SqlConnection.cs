using System.Data.SqlClient;
using Dapper;

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
}