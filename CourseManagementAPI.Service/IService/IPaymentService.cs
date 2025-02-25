using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Service.Base;

namespace CourseManagementAPI.Service.IService;

public interface IPaymentService
{
    Task<Payment?> GetPaymentByIdAsync(string paymentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> GetPaymentsForTrainerAsync(string trainerId, CancellationToken cancellationToken = default);
    Task<Payment> CreatePaymentAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<Payment> UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken = default);
    Task DeletePaymentAsync(string paymentId, CancellationToken cancellationToken = default);
}