using System.Security.Cryptography;
using Application.Application.Commands;
using Application.Contracts;
using Application.Contracts.LoginResult;
using Application.Models;
using Application.Repositories;

namespace Application.Application.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IList<ICommand> _history;
    private User? _currentUser;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _history = new List<ICommand>();
    }

    public User? User => _currentUser;

    public async Task<LoginResult> Login(string username, int password)
    {
        byte[] passwordBytes = BitConverter.GetBytes(password);
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        string passwordHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty, StringComparison.Ordinal);

        User? user = await _userRepository.FindByUsernameAsync(username, passwordHash).ConfigureAwait(false);

        if (user is null || user.UserRole is UserRole.Admin)
        {
            return new Fail();
        }

        _currentUser = user;

        return new Successful();
    }

    public void Execute(ICommand command)
    {
        _history.Add(command);
        command.Execute(this);
    }

    public int GetBalance()
    {
        if (_currentUser is null)
        {
            throw new ArgumentException("User is null");
        }

        return _currentUser.Balance;
    }

    public void GetMoney(int amount)
    {
        if (User is null)
        {
            throw new ArgumentException("User is null");
        }

        if (User.Balance < amount)
        {
            throw new ArgumentException("Not enough balance");
        }

        _currentUser = new User(User.Name, User.Id, User.Balance - amount, User.UserRole, User.Password);
        _userRepository.UpdateUserBalanceAsync(_currentUser);
    }

    public void PutMoney(int amount)
    {
        if (User is null)
        {
            throw new ArgumentException("User is null");
        }

        _currentUser = new User(User.Name, User.Id, User.Balance + amount, User.UserRole, User.Password);
        _userRepository.UpdateUserBalanceAsync(_currentUser);
    }

    public IReadOnlyList<ICommand> CommandHistory()
    {
        return new List<ICommand>(_history);
    }

    public async Task MakeAccount(string name, int password)
    {
        byte[] passwordBytes = BitConverter.GetBytes(password);
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        string passwordHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty, StringComparison.Ordinal);

        var user = new User(name, name.GetHashCode(StringComparison.Ordinal), 0, UserRole.RegularUser, passwordHash);

        await _userRepository.AddAsync(user).ConfigureAwait(false);
    }
}