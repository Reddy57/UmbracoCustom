using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities;

public class AfsUser: IdentityUser<int>
{
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public ICollection<AfsUserRole> RolesForUser { get; set; }
}