using Application.Contracts;

namespace Application.Application.Commands;

public class GetMoneyCommand : ICommand
{
    private readonly int _amount;

    public GetMoneyCommand(int amount)
    {
        _amount = amount;
    }

    public string Name => "Get money";

    public int Value => _amount;

    public void Execute(IUserService userService)
    {
        userService.GetMoney(_amount);
    }
}