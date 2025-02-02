using Application.Models;

namespace Application.Repositories;

public interface IAdminRepository
{
    Task<User?> FindAdmin(string username, string password);
    Task<User?> FindByUsernameAsync(string username);
    Task UpdateUserBalanceAsync(User user);
    Task UpdateUserNameAsync(User user);
    Task UpdateUserPasswordAsync(User user);
    Task AddAsync(User user);
}