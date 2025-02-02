using Application.Application.Commands;
using Application.Models;

namespace Application.Contracts;

public interface IUserService
{
    User? User { get; }
    Task<LoginResult.LoginResult> Login(string username, int password);
    int GetBalance();
    void Execute(ICommand command);
    void PutMoney(int amount);
    void GetMoney(int amount);
    IReadOnlyList<ICommand> CommandHistory();
    Task MakeAccount(string name, int password);
}