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
    private readonly PasswordHasher<AfsUser> _passwordHasher;

    public AccountService(AfsDbContext afsDbContext)
    {
        _afsDbContext = afsDbContext;
        _passwordHasher = new PasswordHasher<AfsUser>();
    }

    public async Task<LoginResponseViewModel> ValidateUser(string email, string password)
    {
        var user = await _afsDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || string.IsNullOrEmpty(user.SecurityStamp))
            return new LoginResponseViewModel { Email = email, IsValid = false };

        var saltedPassword = user.SecurityStamp + password;
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, saltedPassword);
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
        var salt = GenerateSalt();
        var saltedPassword = salt + requestModel.Password;
        var user = new AfsUser
        {
            FirstName = requestModel.
            Email = requestModel.Email,
            SecurityStamp = salt,
            PasswordHash = _passwordHasher.HashPassword(null, saltedPassword)
        };

        _afsDbContext.Users.Add(user);
        var saveResult = await _afsDbContext.SaveChangesAsync();

        return saveResult > 0;
    }

    public async Task<AfsUser> GetUserByEmail(string email)
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