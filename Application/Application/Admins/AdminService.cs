using System.Security.Cryptography;
using Application.Contracts;
using Application.Contracts.LoginResult;
using Application.Models;
using Application.Repositories;

namespace Application.Application.Admins;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private User? _currentAdmin;

    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public User? User => _currentAdmin;

    public async Task<LoginResult> Login(string username, int password)
    {
        byte[] passwordBytes = BitConverter.GetBytes(password);
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        string passwordHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty, StringComparison.Ordinal);

        User? user = await _adminRepository.FindAdmin(username, passwordHash).ConfigureAwait(false);

        if (user is null || user.UserRole is not UserRole.Admin)
        {
            return new Fail();
        }

        _currentAdmin = user;

        return new Successful();
    }

    public async Task<User?> GetUserByName(string name)
    {
        User? user = await _adminRepository.FindByUsernameAsync(name).ConfigureAwait(false);

        if (user is null || user.UserRole is UserRole.Admin)
        {
            return null;
        }

        return user;
    }

    public int GetUserBalance(string name)
    {
        if (_currentAdmin is null)
        {
            throw new ArgumentException("You are not login as admin");
        }

        User? user = _adminRepository.FindByUsernameAsync(name).WaitAsync(CancellationToken.None).Result;

        if (user is null)
        {
            throw new ArgumentException("No such user");
        }

        return user.Balance;
    }

    public void ChangeUserBalance(User user, int newBalance)
    {
        if (_currentAdmin is null)
        {
            throw new ArgumentException("You are not login as admin");
        }

        user = new User(user.Name, user.Id, newBalance, user.UserRole, user.Password);

        _adminRepository.UpdateUserBalanceAsync(user).Wait();
    }

    public void ChangeUserName(User user, string newName)
    {
        if (_currentAdmin is null)
        {
            throw new ArgumentException("You are not login as admin");
        }

        user = new User(newName, user.Id, user.Balance, user.UserRole, user.Password);

        _adminRepository.UpdateUserNameAsync(user).Wait();
    }

    public void ChangeUserPassword(User user, int newPassword)
    {
        if (_currentAdmin is null)
        {
            throw new ArgumentException("You are not login as admin");
        }

        byte[] passwordBytes = BitConverter.GetBytes(newPassword);
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        string passwordHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty, StringComparison.Ordinal);

        user = new User(user.Name, user.Id, user.Balance, user.UserRole, passwordHash);

        _adminRepository.UpdateUserPasswordAsync(user).Wait();
    }
}