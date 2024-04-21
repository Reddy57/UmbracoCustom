namespace ApplicationCore.Entities;

public class UserCredentials
{
    public int Id { get; set; } // Primary key
    public int UserId { get; set; } // Foreign key to User
    public string Password { get; set; }
    public string Salt { get; set; }
    public string Email { get; set; }
    
    public int? FailedPasswordAttemptCount { get; set; }
    public DateTime? LastLockoutDate { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetExpiration { get; set; }
    public bool? HasUpdatedPassword { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public DateTime? LastPasswordChangedDate { get; set; }
    public DateTime? CreatedOnDate { get; set; }

    
    public User User { get; set; } // Navigation property to User


}