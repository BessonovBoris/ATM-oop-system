using System.Globalization;
using Application.Contracts;
using Spectre.Console;

namespace Console.ConsoleHandler.AdminConsoleHandler;

public class ViewUserBalance : IHandler
{
    private readonly IAdminService _adminService;

    public ViewUserBalance(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public string Name => "View balance";

    public void Handle()
    {
        string name = AnsiConsole.Ask<string>("Enter name");

        int balance = _adminService.GetUserBalance(name);

        AnsiConsole.WriteLine(new CultureInfo("es-ES"), balance);
        AnsiConsole.Ask<string>(" ");
    }
}