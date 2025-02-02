using Application.Contracts;
using Application.Models;
using Spectre.Console;

namespace Console.ConsoleHandler.AdminConsoleHandler;

public class ChangeUserBalance : IHandler
{
    private readonly IAdminService _adminService;

    public ChangeUserBalance(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public string Name => "Change balance";

    public void Handle()
    {
        string name = AnsiConsole.Ask<string>("Enter name");
        int balance = AnsiConsole.Ask<int>("Enter new balance");

        User? user = _adminService.GetUserByName(name).WaitAsync(CancellationToken.None).Result;
        if (user is null)
        {
            throw new ArgumentException("There no such user");
        }

        _adminService.ChangeUserBalance(user, balance);
    }
}