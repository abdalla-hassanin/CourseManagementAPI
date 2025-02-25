using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Course.Commands;

public record CreateCourseCommand(
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime EndDate,
    decimal Price,
    int TotalHours,
    int? MaxCapacity,
    string TrainerId
) : IRequest<ApiResponse<CourseDto>>;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
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

public class CreateCourseHandler(
    ICourseService courseService,
    IMapper mapper,
    ILogger<CreateCourseHandler> logger) : IRequestHandler<CreateCourseCommand, ApiResponse<CourseDto>>
{
    public async Task<ApiResponse<CourseDto>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new course with title: {CourseTitle}", request.Title);
        try
        {
            var course = mapper.Map<Data.Entities.Course>(request);
            Data.Entities.Course createdCourse = await courseService.CreateCourseAsync(course, cancellationToken);
            logger.LogInformation("Course created successfully with ID: {CourseId}", createdCourse.CourseId);
            return ApiResponse<CourseDto>.Factory.Created(mapper.Map<CourseDto>(createdCourse));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating course with title: {CourseTitle}", request.Title);
            throw;
        }
    }
}