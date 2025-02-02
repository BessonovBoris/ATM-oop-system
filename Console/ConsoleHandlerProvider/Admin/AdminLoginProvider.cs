using Application.Contracts;
using Console.ConsoleHandler;
using Console.ConsoleHandler.AdminConsoleHandler;

namespace Console.ConsoleHandlerProvider.Admin;

public class AdminLoginProvider : IHandlerProvider
{
    private readonly IAdminService _adminService;

    public AdminLoginProvider(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public IHandler? GetHandler()
    {
        if (_adminService.User is not null)
        {
            return null;
        }

        return new AdminLogin(_adminService);
    }
}