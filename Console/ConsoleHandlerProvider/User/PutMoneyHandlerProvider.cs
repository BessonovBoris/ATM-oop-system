using Application.Contracts;
using Console.ConsoleHandler;
using Console.ConsoleHandler.UserConsoleHandler;

namespace Console.ConsoleHandlerProvider;

public class PutMoneyHandlerProvider : IHandlerProvider
{
    private readonly IUserService _userService;

    public PutMoneyHandlerProvider(IUserService userService)
    {
        _userService = userService;
    }

    public IHandler? GetHandler()
    {
        if (_userService.User is null)
        {
            return null;
        }

        return new PutMoney(_userService);
    }
}