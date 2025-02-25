using System.Net;
using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Course.Queries;

public record SearchCoursesQuery(
    string? SearchTerm,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? OrderBy,
    bool IsDescending,
    int CurrentPage,
    int PageSize
) : IRequest<ApiResponse<IEnumerable<CourseDto>>>;

public class SearchCoursesQueryValidator : AbstractValidator<SearchCoursesQuery>
{
    public SearchCoursesQueryValidator()
    {
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue)
            .WithMessage("Minimum price must be greater than or equal to 0.");
        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue)
            .WithMessage("Maximum price must be greater than or equal to 0.");
        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(x => x.MinPrice).When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue)
            .WithMessage("Maximum price must be greater than or equal to minimum price.");
        RuleFor(x => x.CurrentPage)
            .GreaterThan(0)
            .WithMessage("Current page must be greater than 0.");
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be less than or equal to 100.");
    }
}

public class SearchCoursesHandler(
    ICourseService courseService,
    IMapper mapper,
    ILogger<SearchCoursesHandler> logger) : IRequestHandler<SearchCoursesQuery, ApiResponse<IEnumerable<CourseDto>>>
{
    public async Task<ApiResponse<IEnumerable<CourseDto>>> Handle(SearchCoursesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Searching courses (SearchTerm: {SearchTerm}, MinPrice: {MinPrice}, MaxPrice: {MaxPrice},  Page: {CurrentPage}, PageSize: {PageSize})", 
            request.SearchTerm, request.MinPrice, request.MaxPrice, request.CurrentPage, request.PageSize);
        try
        {
            var courses = await courseService.SearchCoursesAsync(
                request.SearchTerm, request.MinPrice, request.MaxPrice, request.OrderBy, request.IsDescending, request.CurrentPage, request.PageSize,
                cancellationToken);

            var courseDtos = mapper.Map<IEnumerable<CourseDto>>(courses.Items);

            var paginationMetadata = new PaginationMetadata
            {
                CurrentPage = courses.CurrentPage,
                TotalPages = (int)Math.Ceiling(courses.TotalCount / (double)request.PageSize),
                PageSize = request.PageSize,
                TotalCount = courses.TotalCount
            };

            logger.LogInformation("Retrieved {Count} courses matching search criteria", courses.Items.Count);
            return ApiResponse<IEnumerable<CourseDto>>.CreateResponse(
                HttpStatusCode.OK,
                courseDtos,
                "Courses retrieved successfully",
                pagination: paginationMetadata
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while searching courses");
            throw;
        }
    }
}