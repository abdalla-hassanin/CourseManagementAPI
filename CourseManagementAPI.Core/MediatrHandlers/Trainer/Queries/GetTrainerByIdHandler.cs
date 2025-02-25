using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Trainer.Queries;

public record GetTrainerByIdQuery(string TrainerId) : IRequest<ApiResponse<TrainerDto>>;

public class GetTrainerByIdHandler(
    ITrainerService trainerService,
    IMapper mapper,
    ILogger<GetTrainerByIdHandler> logger) : IRequestHandler<GetTrainerByIdQuery, ApiResponse<TrainerDto>>
{
    public async Task<ApiResponse<TrainerDto>> Handle(GetTrainerByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting trainer with ID: {TrainerId}", request.TrainerId);
        try
        {
            var trainer = await trainerService.GetTrainerByIdAsync(request.TrainerId, cancellationToken);
            if (trainer is null)
            {
                logger.LogWarning("Trainer with ID: {TrainerId} not found", request.TrainerId);
                return ApiResponse<TrainerDto>.Factory.NotFound("Trainer not found");
            }
            logger.LogInformation("Trainer with ID: {TrainerId} retrieved successfully", request.TrainerId);
            return ApiResponse<TrainerDto>.Factory.Success(mapper.Map<TrainerDto>(trainer));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting trainer with ID: {TrainerId}", request.TrainerId);
            throw;
        }
    }
}