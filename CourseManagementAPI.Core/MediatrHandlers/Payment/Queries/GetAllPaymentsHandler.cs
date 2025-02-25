using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Payment.Queries;

public record GetAllPaymentsQuery : IRequest<ApiResponse<IReadOnlyList<PaymentDto>>>;

public class GetAllPaymentsHandler(
    IPaymentService paymentService,
    IMapper mapper,
    ILogger<GetAllPaymentsHandler> logger) : IRequestHandler<GetAllPaymentsQuery, ApiResponse<IReadOnlyList<PaymentDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<PaymentDto>>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all payments");
        try
        {
            var payments = await paymentService.GetAllPaymentsAsync(cancellationToken);
            logger.LogInformation("Retrieved {Count} payments", payments.Count);
            return ApiResponse<IReadOnlyList<PaymentDto>>.Factory.Success(mapper.Map<IReadOnlyList<PaymentDto>>(payments));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting all payments");
            throw;
        }
    }
}