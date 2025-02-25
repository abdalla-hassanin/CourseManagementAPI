using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Trainer.Commands;

public record DeleteTrainerCommand(string TrainerId) : IRequest<ApiResponse<bool>>;

public class DeleteTrainerHandler(
    ITrainerService trainerService,
    ILogger<DeleteTrainerHandler> logger) : IRequestHandler<DeleteTrainerCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(DeleteTrainerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting trainer with ID: {TrainerId}", request.TrainerId);
        try
        {
            await trainerService.DeleteTrainerAsync(request.TrainerId, cancellationToken);
            logger.LogInformation("Trainer with ID: {TrainerId} deleted successfully", request.TrainerId);
            return ApiResponse<bool>.Factory.Success(true, "Trainer deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting trainer with ID: {TrainerId}", request.TrainerId);
            throw;
        }
    }
}