using System.Security.Cryptography;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly AfsDbContext _afsDbContext;
    private readonly PasswordHasher<User> _passwordHasher;

    public AccountService(AfsDbContext afsDbContext)
    {
        _afsDbContext = afsDbContext;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<LoginResponseViewModel> ValidateUser(string email, string password)
    {
        var user = await _afsDbContext.Users.Include(u => u.UserCredentials).FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || string.IsNullOrEmpty(user.UserCredentials.Salt))
            return new LoginResponseViewModel { Email = email, IsValid = false };

        var saltedPassword = user.UserCredentials.Salt + password;
        var result = _passwordHasher.VerifyHashedPassword(user, user.UserCredentials.Password, saltedPassword);
        return new LoginResponseViewModel
        {
            Email = email,
            Id = user.Id,
            FirstName = user.FirstName, LastName = user.LastName,
            IsValid = result == PasswordVerificationResult.Success
        };
    }

    public async Task<bool> CreateUser(RegisterViewModel requestModel)
    {
        var user = new User
        {
            FirstName = requestModel.Email = requestModel.Email,
            LastName = requestModel.LastName
        };

        _afsDbContext.Users.Add(user);
        var saveResult = await _afsDbContext.SaveChangesAsync();

        return saveResult > 0;
    }

    public async Task<bool> CreateUserWithPassword(RegisterWithCredentialsViewModel requestModel)
    {
        var user = await _afsDbContext.Users.FirstOrDefaultAsync(u => u.Email == requestModel.Email);

        if (user == null) return false;
        var salt = GenerateSalt();
        var saltedPassword = salt + requestModel.Password;
        var userCredential = new UserCredentials
        {
            UserId = user.Id,
            Email = requestModel.Email,
            Salt = salt,
            Password = _passwordHasher.HashPassword(null, saltedPassword)
        };

        _afsDbContext.UserCredentials.Add(userCredential);
        var saveResult = await _afsDbContext.SaveChangesAsync();

        return saveResult > 0;
    }


    public async Task<User> GetUserByEmail(string email)
    {
        return await _afsDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    private static string GenerateSalt()
    {
        var randomBytes = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }
}