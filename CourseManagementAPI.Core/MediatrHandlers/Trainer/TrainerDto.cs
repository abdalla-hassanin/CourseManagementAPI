namespace CourseManagementAPI.Core.MediatrHandlers.Trainer;


public class TrainerDto
{
    public string TrainerId { get; init; }
    public string ApplicationUserId { get; init; }=string.Empty;
    public string FirstName { get; init; }=string.Empty;
    public string LastName { get; init; }=string.Empty;
    public string Email { get; init; }=string.Empty;
    public string Bio { get; init; }=string.Empty;
}