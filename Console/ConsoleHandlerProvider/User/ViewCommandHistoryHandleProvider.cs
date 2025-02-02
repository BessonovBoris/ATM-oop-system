using Application.Contracts;
using Console.ConsoleHandler;
using Console.ConsoleHandler.UserConsoleHandler;

namespace Console.ConsoleHandlerProvider;

public class ViewCommandHistoryHandleProvider : IHandlerProvider
{
    private readonly IUserService _userService;

    public ViewCommandHistoryHandleProvider(IUserService userService)
    {
        _userService = userService;
    }

    public IHandler? GetHandler()
    {
        if (_userService.User is null)
        {
            return null;
        }

        return new ViewCommandHistory(_userService);
    }
}