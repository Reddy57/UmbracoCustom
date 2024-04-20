using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities;

public class User: IdentityUser<int>
{
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public ICollection<UserRole> RolesForUser { get; set; }
}