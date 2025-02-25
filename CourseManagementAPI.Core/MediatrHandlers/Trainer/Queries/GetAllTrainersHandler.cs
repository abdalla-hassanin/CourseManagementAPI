using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Trainer.Queries;

public record GetAllTrainersQuery : IRequest<ApiResponse<IReadOnlyList<TrainerDto>>>;

public class GetAllTrainersHandler(
    ITrainerService trainerService,
    IMapper mapper,
    ILogger<GetAllTrainersHandler> logger) : IRequestHandler<GetAllTrainersQuery, ApiResponse<IReadOnlyList<TrainerDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<TrainerDto>>> Handle(GetAllTrainersQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all trainers");
        try
        {
            var trainers = await trainerService.GetAllTrainersAsync(cancellationToken);
            logger.LogInformation("Retrieved {Count} trainers", trainers.Count);
            return ApiResponse<IReadOnlyList<TrainerDto>>.Factory.Success(mapper.Map<IReadOnlyList<TrainerDto>>(trainers));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting all trainers");
            throw;
        }
    }
}