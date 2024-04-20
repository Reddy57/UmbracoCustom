using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities;

public class Role: IdentityRole<int>
{
    public ICollection<UserRole> UsersForRole { get; set; }
}