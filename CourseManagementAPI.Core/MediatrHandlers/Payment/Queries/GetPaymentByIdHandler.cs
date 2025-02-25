using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Payment.Queries;

public record GetPaymentByIdQuery(string PaymentId) : IRequest<ApiResponse<PaymentDto>>;

public class GetPaymentByIdHandler(
    IPaymentService paymentService,
    IMapper mapper,
    ILogger<GetPaymentByIdHandler> logger) : IRequestHandler<GetPaymentByIdQuery, ApiResponse<PaymentDto>>
{
    public async Task<ApiResponse<PaymentDto>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting payment with ID: {PaymentId}", request.PaymentId);
        try
        {
            var payment = await paymentService.GetPaymentByIdAsync(request.PaymentId, cancellationToken);
            if (payment is null)
            {
                logger.LogWarning("Payment with ID: {PaymentId} not found", request.PaymentId);
                return ApiResponse<PaymentDto>.Factory.NotFound("Payment not found");
            }
            logger.LogInformation("Payment with ID: {PaymentId} retrieved successfully", request.PaymentId);
            return ApiResponse<PaymentDto>.Factory.Success(mapper.Map<PaymentDto>(payment));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting payment with ID: {PaymentId}", request.PaymentId);
            throw;
        }
    }
}