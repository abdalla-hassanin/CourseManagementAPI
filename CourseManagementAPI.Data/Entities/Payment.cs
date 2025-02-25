namespace CourseManagementAPI.Data.Entities;

public class Payment
{
    public string PaymentId { get; set; }= Ulid.NewUlid().ToString();
    public string TrainerId { get; set; } 
    public string CourseId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public Trainer Trainer { get; set; } 
    public Course Course { get; set; } 
}