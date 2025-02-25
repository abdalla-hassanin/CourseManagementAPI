using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Payment.Commands;

public record DeletePaymentCommand(string PaymentId) : IRequest<ApiResponse<bool>>;

public class DeletePaymentHandler(
    IPaymentService paymentService,
    ILogger<DeletePaymentHandler> logger) : IRequestHandler<DeletePaymentCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting payment with ID: {PaymentId}", request.PaymentId);
        try
        {
            await paymentService.DeletePaymentAsync(request.PaymentId, cancellationToken);
            logger.LogInformation("Payment with ID: {PaymentId} deleted successfully", request.PaymentId);
            return ApiResponse<bool>.Factory.Success(true, "Payment deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting payment with ID: {PaymentId}", request.PaymentId);
            throw;
        }
    }
}