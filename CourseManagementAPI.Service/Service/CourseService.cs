using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Infrastructure.Base;
using CourseManagementAPI.Service.Base;
using CourseManagementAPI.Service.IService;
using CourseManagementAPI.Service.Specification;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Service.Service;

public class CourseService(IUnitOfWork unitOfWork, ILogger<CourseService> logger) : ICourseService
{
    public async Task<Course?> GetCourseByIdAsync(string courseId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting course with ID: {CourseId}", courseId);
        var spec = new CourseSpecifications.ByCourseId(courseId);
        var courses = await unitOfWork.Repository<Course>().ListAsync(spec, cancellationToken);
        return courses.FirstOrDefault();    }

    public async Task<IReadOnlyList<Course>> GetAllCoursesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all courses");
        var spec = new CourseSpecifications.AllCourses();
        return await unitOfWork.Repository<Course>().ListAsync(spec, cancellationToken);    }

    public async Task<PaginationList<Course>> SearchCoursesAsync(
        string? searchTerm,
        decimal? minPrice,
        decimal? maxPrice,
        string? orderBy,
        bool isDescending,
        int currentPage,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Searching courses with parameters - SearchTerm: {SearchTerm}, MinPrice: {MinPrice}, MaxPrice: {MaxPrice}, orderBy={OrderBy}, isDescending={IsDescending}, currentPage={CurrentPage}, pageSize={PageSize}",
            searchTerm, minPrice, maxPrice, orderBy, isDescending, currentPage, pageSize);

        var countSpec = new CourseSpecifications.SearchCount(
            searchTerm,
            minPrice,
            maxPrice);
        var totalCount = (await unitOfWork.Repository<Course>().ListAsync(countSpec, cancellationToken)).Count;
        
        var spec = new CourseSpecifications.Search(searchTerm, minPrice, maxPrice, orderBy, isDescending,
            currentPage, pageSize);
        var courses = await unitOfWork.Repository<Course>().ListAsync(spec, cancellationToken);

        logger.LogInformation("Found {Count} courses", courses.Count);
        return new PaginationList<Course>
        {
            Items = courses,
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };  
    }

    public async Task<Course> CreateCourseAsync(Course course, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating new course with Title: {Title}", course.Title);
        await unitOfWork.Repository<Course>().AddAsync(course, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        logger.LogInformation("Course created successfully with ID: {CourseId}", course.CourseId);
        return course;
    }

    public async Task<Course> UpdateCourseAsync(Course course, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating course with ID: {CourseId}", course.CourseId);
        await unitOfWork.Repository<Course>().UpdateAsync(course, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        logger.LogInformation("Course updated successfully");
        return course;
    }

    public async Task DeleteCourseAsync(string courseId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting course with ID: {CourseId}", courseId);

        var course = await GetCourseByIdAsync(courseId, cancellationToken);
        if (course is not null)
        {
            await unitOfWork.Repository<Course>().DeleteAsync(course, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation("Course deleted successfully");

        }
        else
        {
            logger.LogWarning("Attempted to delete non-existent course with ID: {CourseId}", courseId);

        }

    }

}