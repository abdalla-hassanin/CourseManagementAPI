namespace CourseManagementAPI.Core.MediatrHandlers.Payment;

public record PaymentDto(
    string PaymentId,
    string TrainerId,
    string CourseId,
    decimal Amount,
    DateTime PaymentDate
);