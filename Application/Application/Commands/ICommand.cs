using Application.Contracts;

namespace Application.Application.Commands;

public interface ICommand
{
    string Name { get; }
    int Value { get; }
    void Execute(IUserService userService);
}