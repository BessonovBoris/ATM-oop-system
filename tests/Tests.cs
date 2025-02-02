using System.Security.Cryptography;
using Application.Application.Users;
using Application.Models;
using Application.Repositories;
using NSubstitute;
using Xunit;

namespace tests;

public class Tests
{
    [Fact]
    public void GetMoneyFromBalance()
    {
        // Arrange
        byte[] passwordBytes = BitConverter.GetBytes(123);
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        string passwordHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty, StringComparison.Ordinal);

        IUserRepository userRepository = Substitute.For<IUserRepository>();
        userRepository.FindByUsernameAsync("Ivan", passwordHash).Returns(new User("Ivan", 3, 100, UserRole.RegularUser, passwordHash));

        var userService = new UserService(userRepository);

        // Act
        userService.Login("Ivan", 123).Wait();
        userService.GetMoney(50);

        // Assert
        Assert.Equal(50, userService.User?.Balance);
    }
}