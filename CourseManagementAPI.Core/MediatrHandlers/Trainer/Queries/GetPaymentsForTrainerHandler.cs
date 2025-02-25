using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Core.MediatrHandlers.Payment;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Trainer.Queries;

public record GetPaymentsForTrainerQuery(string TrainerId) : IRequest<ApiResponse<IReadOnlyList<PaymentDto>>>;

public class GetPaymentsForTrainerHandler(
    ITrainerService trainerService,
    IMapper mapper,
    ILogger<GetPaymentsForTrainerHandler> logger) : IRequestHandler<GetPaymentsForTrainerQuery, ApiResponse<IReadOnlyList<PaymentDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<PaymentDto>>> Handle(GetPaymentsForTrainerQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all payments for trainer with ID: {TrainerId}", request.TrainerId);
        try
        {
            var payments = await trainerService.GetPaymentsForTrainerAsync(request.TrainerId, cancellationToken);
            if (!payments.Any())
            {
                logger.LogInformation("No payments found for trainer with ID: {TrainerId}", request.TrainerId);
            }
            logger.LogInformation("Retrieved {Count} payments for trainer with ID: {TrainerId}", payments.Count, request.TrainerId);
            return ApiResponse<IReadOnlyList<PaymentDto>>.Factory.Success(mapper.Map<IReadOnlyList<PaymentDto>>(payments));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting payments for trainer with ID: {TrainerId}", request.TrainerId);
            throw;
        }
    }
}