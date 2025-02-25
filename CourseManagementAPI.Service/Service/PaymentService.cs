using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Infrastructure.Base;
using CourseManagementAPI.Service.IService;
using CourseManagementAPI.Service.Specification;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Service.Service;

public class PaymentService(IUnitOfWork unitOfWork, ILogger<PaymentService> logger) : IPaymentService
{
    public async Task<Payment?> GetPaymentByIdAsync(string paymentId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting payment with ID: {PaymentId}", paymentId);
        var spec = new PaymentSpecifications.ByPaymentId(paymentId);
        var payments = await unitOfWork.Repository<Payment>().ListAsync(spec, cancellationToken);
        return payments.FirstOrDefault();
    }

    public async Task<IReadOnlyList<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all payments");
        var spec = new PaymentSpecifications.AllPayments();
        return await unitOfWork.Repository<Payment>().ListAsync(spec, cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetPaymentsForTrainerAsync(string trainerId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all payments for trainer with ID: {TrainerId}", trainerId);
        var spec = new PaymentSpecifications.ByTrainerId(trainerId);
        return await unitOfWork.Repository<Payment>().ListAsync(spec, cancellationToken);
    }

    public async Task<Payment> CreatePaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating new payment for TrainerId: {TrainerId}, CourseId: {CourseId}", 
            payment.TrainerId, payment.CourseId);
        await unitOfWork.Repository<Payment>().AddAsync(payment, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        logger.LogInformation("Payment created successfully with ID: {PaymentId}", payment.PaymentId);
        return payment;
    }

    public async Task<Payment> UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating payment with ID: {PaymentId}", payment.PaymentId);
        await unitOfWork.Repository<Payment>().UpdateAsync(payment, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        logger.LogInformation("Payment updated successfully");
        return payment;
    }

    public async Task DeletePaymentAsync(string paymentId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting payment with ID: {PaymentId}", paymentId);
        var payment = await GetPaymentByIdAsync(paymentId, cancellationToken);
        if (payment is not null)
        {
            await unitOfWork.Repository<Payment>().DeleteAsync(payment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Payment deleted successfully");
        }
        else
        {
            logger.LogWarning("Attempted to delete non-existent payment with ID: {PaymentId}", paymentId);
        }
    }
}