using Application.Models;

namespace Application.Contracts;

public interface IAdminService
{
    User? User { get; }
    Task<LoginResult.LoginResult> Login(string username, int password);
    Task<User?> GetUserByName(string name);
    int GetUserBalance(string name);
    void ChangeUserBalance(User user, int newBalance);
    void ChangeUserName(User user, string newName);
    void ChangeUserPassword(User user, int newPassword);
}