using Microsoft.AspNetCore.Identity;

namespace CourseManagementAPI.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Id = Ulid.NewUlid().ToString();
    }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public Trainer? Trainer { get; set; }
    public Admin? Admin { get; set; }
}
