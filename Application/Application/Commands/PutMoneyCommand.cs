using Application.Contracts;

namespace Application.Application.Commands;

public class PutMoneyCommand : ICommand
{
    private readonly int _amount;

    public PutMoneyCommand(int amount)
    {
        _amount = amount;
    }

    public string Name => "Put money";

    public int Value => _amount;

    public void Execute(IUserService userService)
    {
        userService.PutMoney(_amount);
    }
}