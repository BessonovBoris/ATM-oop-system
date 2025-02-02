using Console.ConsoleHandler;
using Console.ConsoleHandlerProvider;
using Spectre.Console;

namespace Console;

public class HandlerRunner
{
    private readonly IList<IHandlerProvider> _handlerProviders;

    public HandlerRunner(IList<IHandlerProvider> handlerProviders)
    {
        _handlerProviders = handlerProviders;
    }

    public void Run()
    {
        IList<IHandler> handlers = new List<IHandler>(GetHandlers());

        AnsiConsole.Clear();
        IHandler handler = AnsiConsole.Prompt(
            new SelectionPrompt<IHandler>()
                .Title("What you want to do?")
                .AddChoices(handlers)
                .UseConverter(handler => handler.Name));

        handler.Handle();
    }

    private IEnumerable<IHandler> GetHandlers()
    {
        foreach (IHandlerProvider handlerProvider in _handlerProviders)
        {
            IHandler? handler = handlerProvider.GetHandler();
            if (handler is not null)
            {
                yield return handler;
            }
        }
    }
}