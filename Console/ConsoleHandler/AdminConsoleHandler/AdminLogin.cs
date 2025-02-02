using Application.Contracts;
using Application.Contracts.LoginResult;
using Spectre.Console;

namespace Console.ConsoleHandler.AdminConsoleHandler;

public class AdminLogin : IHandler
{
    private readonly IAdminService _adminService;

    public AdminLogin(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public string Name => "Login";

    public void Handle()
    {
        string name = AnsiConsole.Ask<string>("Enter username");
        int password = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter password?")
                .PromptStyle("white")
                .Secret());

        LoginResult result = _adminService.Login(name, password).WaitAsync(CancellationToken.None).Result;

        if (result is Successful)
        {
            AnsiConsole.Markup("[green]Login successful[/]");
        }
        else
        {
            AnsiConsole.Markup("[red]Login fail[/]");
            Environment.Exit(0);
        }

        AnsiConsole.Ask<string>(" ");
    }
}