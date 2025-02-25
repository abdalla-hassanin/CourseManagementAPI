using System.Linq.Expressions;
using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Infrastructure.Base.Specifications;

namespace CourseManagementAPI.Service.Specification;

public static class CourseSpecifications
{
    public sealed class SearchCount(string? searchTerm, decimal? minPrice, decimal? maxPrice)
        : BaseSpecification<Course>(CreateSearchCriteria(searchTerm, minPrice, maxPrice))
    {
        private static Expression<Func<Course, bool>> CreateSearchCriteria(string? searchTerm, decimal? minPrice,
            decimal? maxPrice)
        {
            return c => (string.IsNullOrEmpty(searchTerm) ||
                         c.Title.ToLower().Contains(searchTerm.ToLower()) ||
                         (c.Description != null && c.Description.ToLower().Contains(searchTerm.ToLower())))
                        && (!minPrice.HasValue || c.Price >= minPrice.Value)
                        && (!maxPrice.HasValue || c.Price <= maxPrice.Value);
        }
    }

    public sealed class Search : BaseSpecification<Course>
    {
        public Search(string? searchTerm, decimal? minPrice, decimal? maxPrice, string? orderBy, bool isDescending,
            int currentPage, int pageSize)
            : base(CreateSearchCriteria(searchTerm, minPrice, maxPrice))
        {
            ApplyPagingAndOrdering(orderBy, isDescending, currentPage, pageSize);
            AddCommonIncludes();
        }

        private static Expression<Func<Course, bool>> CreateSearchCriteria(string? searchTerm, decimal? minPrice,
            decimal? maxPrice)
        {
            return c => (string.IsNullOrEmpty(searchTerm) ||
                         c.Title.ToLower().Contains(searchTerm.ToLower()) ||
                         (c.Description != null && c.Description.ToLower().Contains(searchTerm.ToLower())))
                        && (!minPrice.HasValue || c.Price >= minPrice.Value)
                        && (!maxPrice.HasValue || c.Price <= maxPrice.Value);
        }

        private void ApplyPagingAndOrdering(string? orderBy, bool isDescending, int currentPage, int pageSize)
        {
            ApplyPaging((currentPage - 1) * pageSize, pageSize);

            Expression<Func<Course, object>> orderExpression = orderBy?.ToLower() switch
            {
                "title" => c => c.Title,
                "startdate" => c => c.StartDate,
                "enddate" => c => c.EndDate,
                "hours" => c => c.TotalHours,
                "price" => c => c.Price,
                _ => c => c.CourseId
            };

            if (isDescending)
                ApplyOrderByDescending(orderExpression);
            else
                ApplyOrderBy(orderExpression);
        }

        private void AddCommonIncludes()
        {
            AddInclude(c => c.Trainer);
        }
    }
    public sealed class ByCourseId : BaseSpecification<Course>
    {
        public ByCourseId(string courseId) : base(c => c.CourseId == courseId)
        {
            AddInclude(c => c.Trainer);
        }
    }
    public sealed class AllCourses : BaseSpecification<Course>
    {
        public AllCourses() : base(c => true)
        {
            AddInclude(c => c.Trainer);
            ApplyOrderBy(c => c.CourseId);
        }
    }
    public sealed class ByTrainerId : BaseSpecification<Course>
    {
        public ByTrainerId(string trainerId) : base(c => c.TrainerId == trainerId)
        {
            AddInclude(c => c.Trainer);
            ApplyOrderBy(c => c.StartDate);
        }
    }
    
}