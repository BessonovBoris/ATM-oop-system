using Application.Models;
using Application.Repositories;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Npgsql;

namespace Abstractions.Repository;

public class AdminRepository : IAdminRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;

    public AdminRepository(IPostgresConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<User?> FindAdmin(string username, string password)
    {
        const string sql = """
                           select id, name, balance, role, password
                           from users
                           where name = :username and password = :userPassword;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default).ConfigureAwait(false);

        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("username", username);
        command.AddParameter("userPassword", password);

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

    public async Task<User?> FindByUsernameAsync(string username)
    {
        const string sql = """
                           select id, name, balance, role, password
                           from users
                           where name = :username;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default).ConfigureAwait(false);

        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("username", username);

        NpgsqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        if (await reader.ReadAsync().ConfigureAwait(false) is false)
            return null;

        int id = reader.GetInt32(0);
        string name = reader.GetString(1);
        int balance = reader.GetInt32(2);
        var userRole = (UserRole)reader.GetInt32(3);
        string password = reader.GetString(4);

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
                           where id = :userId;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default).ConfigureAwait(false);

        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("userBalance", user.Balance);
        command.AddParameter("userId", user.Id);

        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task UpdateUserNameAsync(User user)
    {
        const string sql = """
                           UPDATE users
                           SET name = :userName
                           where id = :userId;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default).ConfigureAwait(false);

        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("userName", user.Name);
        command.AddParameter("userId", user.Id);

        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task UpdateUserPasswordAsync(User user)
    {
        const string sql = """
                           UPDATE users
                           SET password = :userPassword
                           where id = :userId;
                           """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(default).ConfigureAwait(false);

        using var command = new NpgsqlCommand(sql, connection);

        command.AddParameter("userPassword", user.Password);
        command.AddParameter("userId", user.Id);

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