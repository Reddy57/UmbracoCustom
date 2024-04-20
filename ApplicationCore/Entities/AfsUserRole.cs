using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities;

public class AfsUserRole: IdentityUserRole<int>
{
    public AfsRole Role { get; set; }
    public AfsUser User { get; set; }
}