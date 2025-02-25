namespace CourseManagementAPI.Core.MediatrHandlers.Course;

public record CourseDto(
    string CourseId,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime EndDate,
    decimal Price,
    int TotalHours,
    int? MaxCapacity,
    string TrainerId
);