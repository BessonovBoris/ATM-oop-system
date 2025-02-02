using Application.Contracts;
using Application.Contracts.LoginResult;
using Spectre.Console;

namespace Console.ConsoleHandler.UserConsoleHandler;

public class UserLogin : IHandler
{
    private readonly IUserService _userService;

    public UserLogin(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Login";

    public void Handle()
    {
        string name = AnsiConsole.Ask<string>("Enter username");
        int password = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter password?")
                .PromptStyle("white")
                .Secret());

        LoginResult result = _userService.Login(name, password).WaitAsync(CancellationToken.None).Result;

        if (result is Successful)
        {
            AnsiConsole.Markup("[green]Login successful[/]");
        }
        else
        {
            AnsiConsole.Markup("[red]Login fail[/]");
        }

        AnsiConsole.Ask<string>(" ");
    }
}