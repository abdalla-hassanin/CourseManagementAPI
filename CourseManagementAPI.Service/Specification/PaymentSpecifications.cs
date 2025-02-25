using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Infrastructure.Base.Specifications;

namespace CourseManagementAPI.Service.Specification;

public static class PaymentSpecifications
{
    public sealed class ByPaymentId : BaseSpecification<Payment>
    {
        public ByPaymentId(string paymentId) : base(p => p.PaymentId == paymentId)
        {
            AddInclude(p => p.Trainer);
            AddInclude(p => p.Course);
        }
    }

    public sealed class AllPayments : BaseSpecification<Payment>
    {
        public AllPayments() : base(p => true)
        {
            AddInclude(p => p.Trainer);
            AddInclude(p => p.Course);
            ApplyOrderBy(p => p.PaymentDate);
        }
    }

    public sealed class ByTrainerId : BaseSpecification<Payment>
    {
        public ByTrainerId(string trainerId) : base(p => p.TrainerId == trainerId)
        {
            AddInclude(p => p.Trainer);
            AddInclude(p => p.Course);
            ApplyOrderBy(p => p.PaymentDate);
        }
    }
}