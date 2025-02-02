using Application.Application.Commands;
using Application.Contracts;
using Spectre.Console;

namespace Console.ConsoleHandler.UserConsoleHandler;

public class ViewCommandHistory : IHandler
{
    private readonly IUserService _userService;

    public ViewCommandHistory(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "View history";

    public void Handle()
    {
        AnsiConsole.Clear();
        var history = new List<ICommand>(_userService.CommandHistory());

        foreach (ICommand command in history)
        {
            AnsiConsole.WriteLine($"{command.Name}: {command.Value}");
        }

        AnsiConsole.Ask<string>(" ");
    }
}