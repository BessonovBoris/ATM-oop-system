using Application.Contracts;
using Application.Models;
using Spectre.Console;

namespace Console.ConsoleHandler.AdminConsoleHandler;

public class ChangeUserName : IHandler
{
    private readonly IAdminService _adminService;

    public ChangeUserName(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public string Name => "Change name";

    public void Handle()
    {
        string name = AnsiConsole.Ask<string>("Enter name");
        string newName = AnsiConsole.Ask<string>("Enter new name");

        User? user = _adminService.GetUserByName(name).WaitAsync(CancellationToken.None).Result;
        if (user is null)
        {
            throw new ArgumentException("There no such user");
        }

        _adminService.ChangeUserName(user, newName);
    }
}