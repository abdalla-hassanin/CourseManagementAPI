using CourseManagementAPI.Api.ResponseExample;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Core.MediatrHandlers.Course;
using CourseManagementAPI.Core.MediatrHandlers.Course.Commands;
using CourseManagementAPI.Core.MediatrHandlers.Course.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CourseManagementAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController(IMediator mediator) : ControllerBase
{
    [HttpGet("{courseId}")]
    [SwaggerOperation(
        Summary = "Get Course by ID",
        Description = "This endpoint allows Admins, Trainers, and public users to retrieve a course by its ID."
    )]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetCourseByIdResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundCourseResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedCourseResponseExample))]
    public async Task<IResult> GetCourseById(
        string courseId,
        CancellationToken cancellationToken)
    {
        var query = new GetCourseByIdQuery(courseId);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToResult();
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Courses",
        Description = "This endpoint allows Admins, Trainers, and public users to retrieve all courses."
    )]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CourseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CourseDto>>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllCoursesResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedCourseResponseExample))]
    public async Task<IResult> GetAllCourses(CancellationToken cancellationToken)
    {
        var query = new GetAllCoursesQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.ToResult();
    }

    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Search Courses",
        Description = "This endpoint allows Admins, Trainers, and public users to search for courses."
    )]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseDto>>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SearchCoursesResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedCourseResponseExample))]
    public async Task<IResult> SearchCourses(
        [FromQuery] string? searchTerm,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string? orderBy,
        [FromQuery] bool isDescending,
        [FromQuery] int currentPage = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchCoursesQuery(searchTerm, minPrice, maxPrice, orderBy, isDescending, currentPage,
            pageSize);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToResult();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Trainer")]
    [SwaggerOperation(
        Summary = "Create New Course",
        Description = "This endpoint allows Admins to create a new course."
    )]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreatedCourseResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestCourseResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedCourseResponseExample))]
    public async Task<IResult> CreateCourse(
        [FromBody] CreateCourseCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.ToResult();
    }

    [HttpPut("{courseId}")]
    [Authorize(Roles = "Admin,Trainer")]
    [SwaggerOperation(
        Summary = "Update Course",
        Description = "This endpoint allows Admins to update a course."
    )]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UpdateCourseResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundCourseResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestCourseResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedCourseResponseExample))]
    public async Task<IResult> UpdateCourse(
        string courseId,
        [FromBody] UpdateCourseCommand command,
        CancellationToken cancellationToken)
    {
        if (courseId != command.CourseId)
        {
            return ApiResponseResults.BadRequest("Course ID mismatch");
        }

        var result = await mediator.Send(command, cancellationToken);
        return result.ToResult();
    }

    [HttpDelete("{courseId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(
        Summary = "Delete Course",
        Description = "This endpoint allows Admins to delete a course."
    )]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteCourseResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedCourseResponseExample))]
    public async Task<IResult> DeleteCourse(
        string courseId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCourseCommand(courseId);
        var result = await mediator.Send(command, cancellationToken);
        return result.ToResult();
    }
}