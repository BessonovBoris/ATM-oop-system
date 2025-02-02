using Application.Contracts;
using Console.ConsoleHandler;
using Console.ConsoleHandler.UserConsoleHandler;

namespace Console.ConsoleHandlerProvider;

public class GetMoneyHandlerProvider : IHandlerProvider
{
    private readonly IUserService _userService;

    public GetMoneyHandlerProvider(IUserService userService)
    {
        _userService = userService;
    }

    public IHandler? GetHandler()
    {
        if (_userService.User is null)
        {
            return null;
        }

        return new GetMoney(_userService);
    }
}