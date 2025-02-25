using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Course.Commands;

public record DeleteCourseCommand(string CourseId) : IRequest<ApiResponse<bool>>;

public class DeleteCourseHandler(
    ICourseService courseService,
    ILogger<DeleteCourseHandler> logger) : IRequestHandler<DeleteCourseCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting course with ID: {CourseId}", request.CourseId);
        try
        {
            await courseService.DeleteCourseAsync(request.CourseId, cancellationToken);
            logger.LogInformation("Course with ID: {CourseId} deleted successfully", request.CourseId);
            return ApiResponse<bool>.Factory.Success(true, "Course deleted successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting course with ID: {CourseId}", request.CourseId);
            throw;
        }
    }
}