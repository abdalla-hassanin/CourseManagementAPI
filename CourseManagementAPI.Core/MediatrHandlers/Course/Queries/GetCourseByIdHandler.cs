using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Course.Queries;

public record GetCourseByIdQuery(string CourseId) : IRequest<ApiResponse<CourseDto>>;

public class GetCourseByIdHandler(
    ICourseService courseService,
    IMapper mapper,
    ILogger<GetCourseByIdHandler> logger) : IRequestHandler<GetCourseByIdQuery, ApiResponse<CourseDto>>
{
    public async Task<ApiResponse<CourseDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting course with ID: {CourseId}", request.CourseId);
        try
        {
            var course = await courseService.GetCourseByIdAsync(request.CourseId, cancellationToken);
            if (course is null)
            {
                logger.LogWarning("Course with ID: {CourseId} not found", request.CourseId);
                return ApiResponse<CourseDto>.Factory.NotFound("Course not found");
            }
            logger.LogInformation("Course with ID: {CourseId} retrieved successfully", request.CourseId);
            return ApiResponse<CourseDto>.Factory.Success(mapper.Map<CourseDto>(course));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting course with ID: {CourseId}", request.CourseId);
            throw;
        }
    }
}