using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool? IsSuperUser { get; set; }
    public string? DisplayName { get; set; }
    public string? LastIpAddress { get; set; }
    public bool? IsDeleted { get; set; }
    public int? CreatedByUserId { get; set; }
    public bool? IsLockedOut { get; set; }
    public bool? IsApproved { get; set; }
    
    public DateTime? CreatedOnDate { get; set; }
    public int? LastModifiedByUserId { get; set; }
    public DateTime? LastModifiedOnDate { get; set; }


    // Navigation property to UserPassword
    
    public UserCredentials UserCredentials { get; set; }


 
}