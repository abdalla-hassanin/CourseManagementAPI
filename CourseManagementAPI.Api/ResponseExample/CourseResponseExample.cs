using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Core.MediatrHandlers.Course;
using Swashbuckle.AspNetCore.Filters;

namespace CourseManagementAPI.Api.ResponseExample;

public class GetCourseByIdResponseExample : IExamplesProvider<ApiResponse<CourseDto>>
{
    public ApiResponse<CourseDto> GetExamples()
    {
        var courseDto = new CourseDto(
            CourseId: "01HF3WFKX1KPY89WNJRXJ6V18N",
            Title: "Sample Course",
            Description: "This is a sample course description.",
            StartDate: DateTime.UtcNow.AddDays(5),
            EndDate: DateTime.UtcNow.AddDays(10),
            Price: 199.99m,
            TotalHours: 20,
            MaxCapacity: 30,
            TrainerId: "01HF3WFKX1KPY89WNJRXJ6V18N"
        );

        return ApiResponse<CourseDto>.Factory.Success(courseDto);
    }
}

public class GetAllCoursesResponseExample : IExamplesProvider<ApiResponse<IReadOnlyList<CourseDto>>>
{
    public ApiResponse<IReadOnlyList<CourseDto>> GetExamples()
    {
        var courses = new List<CourseDto>
        {
            new CourseDto("01HF3WFKX1KPY89WNJRXJ6V18N", "Course 1", "Description 1", DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(10), 199.99m, 20, 30, "01HF3WFKX1KPY89WNJRXJ6V18N"),
            new CourseDto("01HF3WFKX1KPY89WNJRXJ6V18M", "Course 2", "Description 2", DateTime.UtcNow.AddDays(15), DateTime.UtcNow.AddDays(20), 299.99m, 30, 25, "01HF3WFKX1KPY89WNJRXJ6V18M")
        };

        return ApiResponse<IReadOnlyList<CourseDto>>.Factory.Success(courses);
    }
}

public class SearchCoursesResponseExample : IExamplesProvider<ApiResponse<IEnumerable<CourseDto>>>
{
    public ApiResponse<IEnumerable<CourseDto>> GetExamples()
    {
        var courses = new List<CourseDto>
        {
            new CourseDto("01HF3WFKX1KPY89WNJRXJ6V18N", "Search Result 1", "Description 1", DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(10), 199.99m, 20, 30, "01HF3WFKX1KPY89WNJRXJ6V18N"),
            new CourseDto("01HF3WFKX1KPY89WNJRXJ6V18M", "Search Result 2", "Description 2", DateTime.UtcNow.AddDays(15), DateTime.UtcNow.AddDays(20), 299.99m, 30, 25, "01HF3WFKX1KPY89WNJRXJ6V18M")
        };

        var pagination = new PaginationMetadata
        {
            CurrentPage = 1,
            TotalPages = 5,
            PageSize = 10,
            TotalCount = 50
        };

        return ApiResponse<IEnumerable<CourseDto>>.Factory.WithPagination(courses, pagination, "Search results retrieved successfully");
    }
}

public class CreatedCourseResponseExample : IExamplesProvider<ApiResponse<CourseDto>>
{
    public ApiResponse<CourseDto> GetExamples()
    {
        var courseDto = new CourseDto(
            CourseId: "01HF3WFKX1KPY89WNJRXJ6V18N",
            Title: "New Course",
            Description: "This is a newly created course.",
            StartDate: DateTime.UtcNow.AddDays(7),
            EndDate: DateTime.UtcNow.AddDays(14),
            Price: 149.99m,
            TotalHours: 15,
            MaxCapacity: 20,
            TrainerId: "01HF3WFKX1KPY89WNJRXJ6V18N"
        );

        return ApiResponse<CourseDto>.Factory.Created(courseDto, "Course created successfully");
    }
}

public class UpdateCourseResponseExample : IExamplesProvider<ApiResponse<CourseDto>>
{
    public ApiResponse<CourseDto> GetExamples()
    {
        var courseDto = new CourseDto(
            CourseId: "01HF3WFKX1KPY89WNJRXJ6V18N",
            Title: "Updated Course",
            Description: "This course has been updated.",
            StartDate: DateTime.UtcNow.AddDays(10),
            EndDate: DateTime.UtcNow.AddDays(17),
            Price: 179.99m,
            TotalHours: 18,
            MaxCapacity: 25,
            TrainerId: "01HF3WFKX1KPY89WNJRXJ6V18N"
        );

        return ApiResponse<CourseDto>.Factory.Success(courseDto, "Course updated successfully");
    }
}

public class DeleteCourseResponseExample : IExamplesProvider<ApiResponse<bool>>
{
    public ApiResponse<bool> GetExamples()
    {
        return ApiResponse<bool>.Factory.Success(true, "Course deleted successfully");
    }
}

public class BadRequestCourseResponseExample : IExamplesProvider<ApiResponse<CourseDto>>
{
    public ApiResponse<CourseDto> GetExamples()
    {
        return ApiResponse<CourseDto>.Factory.BadRequest(
            "Invalid course data",
            new List<string> { "Title is required", "Total hours must be greater than 0" }
        );
    }
}

public class UnauthorizedCourseResponseExample : IExamplesProvider<ApiResponse<CourseDto>>
{
    public ApiResponse<CourseDto> GetExamples()
    {
        return ApiResponse<CourseDto>.Factory.Unauthorized("Unauthorized access");
    }
}

public class NotFoundCourseResponseExample : IExamplesProvider<ApiResponse<CourseDto>>
{
    public ApiResponse<CourseDto> GetExamples()
    {
        return ApiResponse<CourseDto>.Factory.NotFound("Course not found");
    }
}