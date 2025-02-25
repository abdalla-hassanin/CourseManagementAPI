
using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Service.Base;

namespace CourseManagementAPI.Service.IService;

public interface ITrainerService
{
    Task<Trainer?> GetTrainerByIdAsync(string trainerId, CancellationToken cancellationToken = default);
    Task<Trainer?> GetTrainerByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Trainer>> GetAllTrainersAsync(CancellationToken cancellationToken = default);
    Task<Trainer> CreateTrainerAsync(Trainer trainer, CancellationToken cancellationToken = default);
    Task<Trainer> UpdateTrainerAsync(Trainer trainer, CancellationToken cancellationToken = default);
    Task DeleteTrainerAsync(string trainerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Course>> GetCoursesForTrainerAsync(string trainerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Payment>> GetPaymentsForTrainerAsync(string trainerId, CancellationToken cancellationToken = default);
}