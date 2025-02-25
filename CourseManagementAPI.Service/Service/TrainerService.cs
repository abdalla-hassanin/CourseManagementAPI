using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Infrastructure.Base;
using CourseManagementAPI.Service.IService;
using CourseManagementAPI.Service.Specification;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Service.Service;

public class TrainerService(IUnitOfWork unitOfWork, ILogger<TrainerService> logger) : ITrainerService
{
    public async Task<Trainer?> GetTrainerByIdAsync(string trainerId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting trainer with ID: {TrainerId}", trainerId);
        var spec = new TrainerSpecifications.ByTrainerId(trainerId);
        var trainers = await unitOfWork.Repository<Trainer>().ListAsync(spec, cancellationToken);
        return trainers.FirstOrDefault();    }
    public async Task<Trainer?> GetTrainerByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting trainer by user ID: {UserId}", userId);
        var spec = new TrainerSpecifications.ByUserId(userId);
        var trainers = await unitOfWork.Repository<Trainer>().ListAsync(spec, cancellationToken);
        return trainers.FirstOrDefault();
    }
    public async Task<IReadOnlyList<Trainer>> GetAllTrainersAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all trainers");
        var spec = new TrainerSpecifications.AllTrainers();
        return await unitOfWork.Repository<Trainer>().ListAsync(spec, cancellationToken);    }

    public async Task<Trainer> CreateTrainerAsync(Trainer trainer, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating new trainer with ApplicationUserId: {ApplicationUserId}", trainer.ApplicationUserId);
        await unitOfWork.Repository<Trainer>().AddAsync(trainer, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        logger.LogInformation("Trainer created successfully with ID: {TrainerId}", trainer.TrainerId);
        return trainer;
    }

    public async Task<Trainer> UpdateTrainerAsync(Trainer trainer, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating trainer with ID: {TrainerId}", trainer.TrainerId);
        await unitOfWork.Repository<Trainer>().UpdateAsync(trainer, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        logger.LogInformation("Trainer updated successfully");
        return trainer;
    }

    public async Task DeleteTrainerAsync(string trainerId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting trainer with ID: {TrainerId}", trainerId);
        var trainer = await GetTrainerByIdAsync(trainerId, cancellationToken);
        if (trainer is not null)
        {
            await unitOfWork.Repository<Trainer>().DeleteAsync(trainer, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation("Trainer deleted successfully");
        }
        else
        {
            logger.LogWarning("Attempted to delete non-existent trainer with ID: {TrainerId}", trainerId);
        }
    }
    
    public async Task<IReadOnlyList<Course>> GetCoursesForTrainerAsync(string trainerId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all courses for trainer with ID: {TrainerId}", trainerId);
        var spec = new CourseSpecifications.ByTrainerId(trainerId);
        return await unitOfWork.Repository<Course>().ListAsync(spec, cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetPaymentsForTrainerAsync(string trainerId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting all payments for trainer with ID: {TrainerId}", trainerId);
        var spec = new PaymentSpecifications.ByTrainerId(trainerId);
        return await unitOfWork.Repository<Payment>().ListAsync(spec, cancellationToken);
    }
}