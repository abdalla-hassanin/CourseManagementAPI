namespace CourseManagementAPI.Data.Entities;

public class Trainer
{
    public string TrainerId { get; set; }= Ulid.NewUlid().ToString();
    public  string ApplicationUserId { get; set; }
    public  ApplicationUser ApplicationUser { get; set; } = null!;
    public string Bio { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>(); 
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}