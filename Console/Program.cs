using Abstractions.Extensions;
using Application.Application.Extensions;
using Application.Contracts;
using Console;
using Console.ConsoleHandlerProvider;
using Console.ConsoleHandlerProvider.Admin;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

IServiceCollection service = new ServiceCollection();

service
    .AddDataAccess(configuration =>
    {
        configuration.Host = "localhost";
        configuration.Port = 5432;
        configuration.Username = "postgres";
        configuration.Password = "1234";
        configuration.Database = "OOP";
        configuration.SslMode = "Prefer";
    })
    .AddApplication();

IServiceProvider provider = service.BuildServiceProvider();
using IServiceScope scope = provider.CreateScope();

string userType = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("You are:")
        .PageSize(10)
        .AddChoices(new[]
        {
            "User", "Admin",
        }));

IList<IHandlerProvider> providers = new List<IHandlerProvider>();

if (userType == "User")
{
    IUserService userService = scope.ServiceProvider.GetRequiredService<IUserService>();

    providers.Add(new LoginHandlerProvider(userService));
    providers.Add(new GetMoneyHandlerProvider(userService));
    providers.Add(new PutMoneyHandlerProvider(userService));
    providers.Add(new ViewBalanceHandlerProvider(userService));
    providers.Add(new MakeNewAccountHandlerProvider(userService));
    providers.Add(new ViewCommandHistoryHandleProvider(userService));
}
else
{
    IAdminService adminService = scope.ServiceProvider.GetRequiredService<IAdminService>();

    providers.Add(new AdminLoginProvider(adminService));
    providers.Add(new ChangeUserBalanceProvider(adminService));
    providers.Add(new ChangeUserNameProvider(adminService));
    providers.Add(new ChangeUserPasswordProvider(adminService));
    providers.Add(new ViewUserBalanceProvider(adminService));
}

var runner = new HandlerRunner(providers);

while (true)
{
    runner.Run();
}
