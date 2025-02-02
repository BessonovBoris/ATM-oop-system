using Application.Contracts;
using Spectre.Console;

namespace Console.ConsoleHandler.UserConsoleHandler;

public class MakeNewAccount : IHandler
{
    private readonly IUserService _userService;

    public MakeNewAccount(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Make new account";

    public void Handle()
    {
        string name = AnsiConsole.Ask<string>("Enter username");
        int password = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter password")
                .PromptStyle("white")
                .Secret());

        _userService.MakeAccount(name, password);
    }
}