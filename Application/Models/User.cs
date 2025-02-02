namespace Application.Models;

public record User(string Name, int Id, int Balance, UserRole UserRole, string Password);