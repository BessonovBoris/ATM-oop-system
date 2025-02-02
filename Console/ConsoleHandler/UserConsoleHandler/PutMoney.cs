using Application.Application.Commands;
using Application.Contracts;
using Spectre.Console;

namespace Console.ConsoleHandler.UserConsoleHandler;

public class PutMoney : IHandler
{
    private readonly IUserService _userService;

    public PutMoney(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Put money";

    public void Handle()
    {
        int amount = AnsiConsole.Ask<int>("How much money would you like to put?");

        _userService.Execute(new PutMoneyCommand(amount));
        AnsiConsole.Ask<string>("[green]Ok[/]");
    }
}