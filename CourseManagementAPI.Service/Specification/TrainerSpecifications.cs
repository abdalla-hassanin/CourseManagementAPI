
using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Infrastructure.Base.Specifications;

namespace CourseManagementAPI.Service.Specification;

public static class TrainerSpecifications
{
    public sealed class ByTrainerId : BaseSpecification<Trainer>
    {
        public ByTrainerId(string trainerId) : base(t => t.TrainerId == trainerId)
        {
            AddInclude(t => t.ApplicationUser);
            AddInclude(t => t.Courses);
            AddInclude(t => t.Payments);
        }
    }

    public sealed class ByUserId : BaseSpecification<Trainer>
    {
        public ByUserId(string userId) : base(t => t.ApplicationUserId == userId)
        {
            AddInclude(t => t.ApplicationUser);
        }
    }

    public sealed class AllTrainers : BaseSpecification<Trainer>
    {
        public AllTrainers() : base(t => true)
        {
            AddInclude(t => t.ApplicationUser);
            ApplyOrderBy(t => t.TrainerId);
        }
    }
}