using System.Globalization;
using Application.Contracts;
using Spectre.Console;

namespace Console.ConsoleHandler.UserConsoleHandler;

public class ViewBalance : IHandler
{
    private readonly IUserService _userService;

    public ViewBalance(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "View balance";

    public void Handle()
    {
        int balance = _userService.GetBalance();
        AnsiConsole.WriteLine(new CultureInfo("es-ES"), balance);
        AnsiConsole.Ask<string>(" ");
    }
}