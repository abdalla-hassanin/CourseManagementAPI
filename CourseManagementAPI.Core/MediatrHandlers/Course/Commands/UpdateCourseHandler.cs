using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Course.Commands;

public record UpdateCourseCommand(
    string CourseId,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime EndDate,
    decimal Price,
    int TotalHours,
    int? MaxCapacity,
    string TrainerId
) : IRequest<ApiResponse<CourseDto>>;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required.")
            .Length(26).WithMessage("Course ID must be a valid ULID (26 characters).")
            .Must(id => Ulid.TryParse(id, out _)).WithMessage("Course ID must be a valid ULID format.");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");
        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date.");
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");
        RuleFor(x => x.TotalHours)
            .GreaterThan(0).WithMessage("Total hours must be greater than 0.");
        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).When(x => x.MaxCapacity.HasValue)
            .WithMessage("Max capacity must be greater than 0 when provided.");
        RuleFor(x => x.TrainerId)
            .NotEmpty().WithMessage("Trainer ID is required.")
            .Length(26).WithMessage("Trainer ID must be a valid ULID (26 characters).")
            .Must(id => Ulid.TryParse(id, out _)).WithMessage("Trainer ID must be a valid ULID format.");
    }
}

public class UpdateCourseHandler(
    ICourseService courseService,
    IMapper mapper,
    ILogger<UpdateCourseHandler> logger) : IRequestHandler<UpdateCourseCommand, ApiResponse<CourseDto>>
{
    public async Task<ApiResponse<CourseDto>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating course with ID: {CourseId}", request.CourseId);
        try
        {
            var existingCourse = await courseService.GetCourseByIdAsync(request.CourseId, cancellationToken);
            if (existingCourse is null)
            {
                logger.LogWarning("Course with ID: {CourseId} not found", request.CourseId);
                return ApiResponse<CourseDto>.Factory.NotFound("Course not found");
            }

            mapper.Map(request, existingCourse);
            var updatedCourse = await courseService.UpdateCourseAsync(existingCourse, cancellationToken);
            logger.LogInformation("Course with ID: {CourseId} updated successfully", request.CourseId);
            return ApiResponse<CourseDto>.Factory.Success(mapper.Map<CourseDto>(updatedCourse));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating course with ID: {CourseId}", request.CourseId);
            throw;
        }
    }
}