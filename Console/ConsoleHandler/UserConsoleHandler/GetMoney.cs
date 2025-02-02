using Application.Application.Commands;
using Application.Contracts;
using Spectre.Console;

namespace Console.ConsoleHandler.UserConsoleHandler;

public class GetMoney : IHandler
{
    private readonly IUserService _userService;

    public GetMoney(IUserService userService)
    {
        _userService = userService;
    }

    public string Name => "Get money";

    public void Handle()
    {
        int amount = AnsiConsole.Ask<int>("How much money would you like to get?");

        try
        {
            _userService.Execute(new GetMoneyCommand(amount));
        }
        catch (ArgumentException e)
        {
            System.Console.WriteLine(e);
        }

        AnsiConsole.Ask<string>("[green]Ok[/]");
    }
}