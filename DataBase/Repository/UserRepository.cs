using Application.Models;
using Application.Repositories;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Npgsql;

namespace Abstractions.Repository;

public class UserRepository : IUserRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public UserRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<User?> FindByUsernameAsync(string username, string password)
    {
        const string sql = """
                           select id, name, balance, role, password
                           from users
                           where name = :username and password = :password;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default).ConfigureAwait(false);

        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("username", username);
        command.AddParameter("password", password);

        NpgsqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        if (await reader.ReadAsync().ConfigureAwait(false) is false)
            return null;

        int id = reader.GetInt32(0);
        string name = reader.GetString(1);
        int balance = reader.GetInt32(2);
        var userRole = (UserRole)reader.GetInt32(3);

        await reader.CloseAsync().ConfigureAwait(false);

        if (!Enum.IsDefined(typeof(UserRole), userRole))
        {
            throw new ArgumentException("There are no such user role");
        }

        return new User(
            Id: id,
            Name: name,
            Balance: balance,
            UserRole: userRole,
            Password: password);
    }

    public async Task UpdateUserBalanceAsync(User user)
    {
        const string sql = """
                        UPDATE users
                        SET balance = :userBalance
                        where id = :userId and password = :password;
                        """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default).ConfigureAwait(false);

        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("userBalance", user.Balance);
        command.AddParameter("userId", user.Id);
        command.AddParameter("username", user.Name);
        command.AddParameter("password", user.Password);

        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task AddAsync(User user)
    {
        const string sql = """
                           INSERT INTO users (name, balance, id, role, password)
                           values(:userName, :userBalance, :userId, :userRole, :password);
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default).ConfigureAwait(false);

        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("userName", user.Name);
        command.AddParameter("userBalance", user.Balance);
        command.AddParameter("userId", user.Id);
        command.AddParameter("userRole", (int)user.UserRole);
        command.AddParameter("password", user.Password);

        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
}