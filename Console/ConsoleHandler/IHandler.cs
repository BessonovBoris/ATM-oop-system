namespace Console.ConsoleHandler;

public interface IHandler
{
    string Name { get;  }
    void Handle();
}