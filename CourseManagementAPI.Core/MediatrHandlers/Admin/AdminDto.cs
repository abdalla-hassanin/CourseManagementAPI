namespace CourseManagementAPI.Core.MediatrHandlers.Admin;
public record AdminDto
{
    public string AdminId { get; init; }
    public string ApplicationUserId { get; init; } = string.Empty;
    public string Position { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}