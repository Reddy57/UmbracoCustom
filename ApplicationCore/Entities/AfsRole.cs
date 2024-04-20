using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities;

public class AfsRole: IdentityRole<int>
{
    public ICollection<AfsUserRole> UsersForRole { get; set; }
}