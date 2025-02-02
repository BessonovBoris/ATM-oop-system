using Application.Contracts;
using Console.ConsoleHandler;
using Console.ConsoleHandler.UserConsoleHandler;

namespace Console.ConsoleHandlerProvider;

public class ViewBalanceHandlerProvider : IHandlerProvider
{
    private readonly IUserService _userService;

    public ViewBalanceHandlerProvider(IUserService userService)
    {
        _userService = userService;
    }

    public IHandler? GetHandler()
    {
        if (_userService.User is null)
        {
            return null;
        }

        return new ViewBalance(_userService);
    }
}