using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Service.Base;

namespace CourseManagementAPI.Service.IService;

public interface ICourseService
{
    Task<Course?> GetCourseByIdAsync(string courseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Course>> GetAllCoursesAsync(CancellationToken cancellationToken = default);
    Task<PaginationList<Course>> SearchCoursesAsync( string? searchTerm, decimal? minPrice, decimal? maxPrice, 
        string? orderBy, bool isDescending, int currentPage , int pageSize, 
        CancellationToken cancellationToken = default);
    Task<Course> CreateCourseAsync(Course course, CancellationToken cancellationToken = default);
    Task<Course> UpdateCourseAsync(Course course, CancellationToken cancellationToken = default);
    Task DeleteCourseAsync(string courseId, CancellationToken cancellationToken = default);
        
    
}