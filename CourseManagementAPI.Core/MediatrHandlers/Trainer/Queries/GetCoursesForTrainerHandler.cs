using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Core.MediatrHandlers.Course;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Trainer.Queries;

public record GetCoursesForTrainerQuery(string TrainerId) : IRequest<ApiResponse<IReadOnlyList<CourseDto>>>;

public class GetCoursesForTrainerHandler(
    ITrainerService trainerService,
    IMapper mapper,
    ILogger<GetCoursesForTrainerHandler> logger) : IRequestHandler<GetCoursesForTrainerQuery, ApiResponse<IReadOnlyList<CourseDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<CourseDto>>> Handle(GetCoursesForTrainerQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all courses for trainer with ID: {TrainerId}", request.TrainerId);
        try
        {
            var courses = await trainerService.GetCoursesForTrainerAsync(request.TrainerId, cancellationToken);
            if (!courses.Any())
            {
                logger.LogInformation("No courses found for trainer with ID: {TrainerId}", request.TrainerId);
            }
            logger.LogInformation("Retrieved {Count} courses for trainer with ID: {TrainerId}", courses.Count, request.TrainerId);
            return ApiResponse<IReadOnlyList<CourseDto>>.Factory.Success(mapper.Map<IReadOnlyList<CourseDto>>(courses));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting courses for trainer with ID: {TrainerId}", request.TrainerId);
            throw;
        }
    }
}