using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Course.Queries;
public record GetAllCoursesQuery : IRequest<ApiResponse<IReadOnlyList<CourseDto>>>;

public class GetAllCoursesHandler(
    ICourseService courseService,
    IMapper mapper,
    ILogger<GetAllCoursesHandler> logger) : IRequestHandler<GetAllCoursesQuery, ApiResponse<IReadOnlyList<CourseDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<CourseDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all courses");
        try
        {
            var courses = await courseService.GetAllCoursesAsync(cancellationToken);
            logger.LogInformation("Retrieved {Count} courses", courses.Count);
            return ApiResponse<IReadOnlyList<CourseDto>>.Factory.Success(mapper.Map<IReadOnlyList<CourseDto>>(courses));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting all courses");
            throw;
        }
    }
}