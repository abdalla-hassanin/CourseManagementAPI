using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Trainer.Commands;

public record UpdateTrainerCommand(
    string TrainerId,
    string Bio
) : IRequest<ApiResponse<TrainerDto>>;

public class UpdateTrainerCommandValidator : AbstractValidator<UpdateTrainerCommand>
{
    public UpdateTrainerCommandValidator()
    {
        RuleFor(x => x.TrainerId)
            .NotEmpty().WithMessage("Trainer ID is required.")
            .Length(26).WithMessage("Trainer ID must be a valid ULID (26 characters).")
            .Must(id => Ulid.TryParse(id, out _)).WithMessage("Trainer ID must be a valid ULID format.");
        RuleFor(x => x.Bio).NotEmpty().WithMessage("Bio is required.");
    }
}

public class UpdateTrainerHandler(
    ITrainerService trainerService,
    IMapper mapper,
    ILogger<UpdateTrainerHandler> logger) : IRequestHandler<UpdateTrainerCommand, ApiResponse<TrainerDto>>
{
    public async Task<ApiResponse<TrainerDto>> Handle(UpdateTrainerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating trainer with ID: {TrainerId}", request.TrainerId);
        try
        {
            var existingTrainer = await trainerService.GetTrainerByIdAsync(request.TrainerId, cancellationToken);
            if (existingTrainer is null)
            {
                logger.LogWarning("Trainer with ID: {TrainerId} not found", request.TrainerId);
                return ApiResponse<TrainerDto>.Factory.NotFound("Trainer not found");
            }

            mapper.Map(request, existingTrainer);
            var updatedTrainer = await trainerService.UpdateTrainerAsync(existingTrainer, cancellationToken);
            logger.LogInformation("Trainer with ID: {TrainerId} updated successfully", request.TrainerId);
            return ApiResponse<TrainerDto>.Factory.Success(mapper.Map<TrainerDto>(updatedTrainer));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating trainer with ID: {TrainerId}", request.TrainerId);
            throw;
        }
    }
}