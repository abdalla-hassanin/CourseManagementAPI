namespace CourseManagementAPI.Data.Entities;

public class Course
{
    public string CourseId { get; set; }= Ulid.NewUlid().ToString();
    public required string Title { get; set; }
    public string? Description { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public int TotalHours { get; set; }
    public int? MaxCapacity { get; set; }

    public string TrainerId { get; set; }
    public Trainer Trainer { get; set; } 
}