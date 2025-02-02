using Application.Contracts;
using Console.ConsoleHandler;
using Console.ConsoleHandler.UserConsoleHandler;

namespace Console.ConsoleHandlerProvider;

public class MakeNewAccountHandlerProvider : IHandlerProvider
{
    private readonly IUserService _userService;

    public MakeNewAccountHandlerProvider(IUserService userService)
    {
        _userService = userService;
    }

    public IHandler? GetHandler()
    {
        if (_userService.User is not null)
        {
            return null;
        }

        return new MakeNewAccount(_userService);
    }
}