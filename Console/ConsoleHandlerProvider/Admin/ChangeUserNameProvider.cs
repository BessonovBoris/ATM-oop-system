﻿using Application.Contracts;
using Console.ConsoleHandler;
using Console.ConsoleHandler.AdminConsoleHandler;

namespace Console.ConsoleHandlerProvider.Admin;

public class ChangeUserNameProvider : IHandlerProvider
{
    private readonly IAdminService _adminService;

    public ChangeUserNameProvider(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public IHandler? GetHandler()
    {
        if (_adminService.User is null)
        {
            return null;
        }

        return new ChangeUserName(_adminService);
    }
}