using ApplicationCore.Entities;
using ApplicationCore.Models;

namespace ApplicationCore.Contracts.Services;

public interface IAccountService
{
    Task<LoginResponseViewModel> ValidateUser(string email, string password);
    Task<bool> CreateUser(RegisterViewModel requestModel);
    Task<bool> CreateUserWithPassword(RegisterViewModel requestModel);
    
    Task<User> GetUserByEmail(string email);
}