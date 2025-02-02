using Console.ConsoleHandler;

namespace Console.ConsoleHandlerProvider;

public interface IHandlerProvider
{
    IHandler? GetHandler();
}