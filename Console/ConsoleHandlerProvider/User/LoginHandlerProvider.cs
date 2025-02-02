using Application.Contracts;
using Console.ConsoleHandler;
using Console.ConsoleHandler.UserConsoleHandler;

namespace Console.ConsoleHandlerProvider;

public class LoginHandlerProvider : IHandlerProvider
{
    private readonly IUserService _userService;

    public LoginHandlerProvider(IUserService userService)
    {
        _userService = userService;
    }

    public IHandler? GetHandler()
    {
        if (_userService.User is not null)
        {
            return null;
        }

        return new UserLogin(_userService);
    }
}