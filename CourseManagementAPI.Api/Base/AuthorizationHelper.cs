using System.Security.Claims;
using CourseManagementAPI.Service.IService;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace CourseManagementAPI.Api.Base;

public static class AuthorizationHelper
{
    public static async Task<bool> CanAccess(ClaimsPrincipal user, string trainerId, ITrainerService trainerService,
        ILogger logger)
    {
        // If user is Admin, they can access everything
        if (user.IsInRole("Admin"))
        {
            logger.LogInformation("User is admin, access granted");
            return true;
        }

        // Get the user ID using the same claim type used in generation
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        logger.LogInformation("Authorization attempt - Found claims:");
        foreach (var claim in user.Claims)
        {
            logger.LogInformation("Claim: {Type} = {Value}", claim.Type, claim.Value);
        }

        logger.LogInformation("Attempting authorization - User ID: {UserId}, Target Trainer ID: {TrainerId}",
            userId, trainerId);

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogWarning("Authorization failed - User ID claim not found. Available claims: {@Claims}",
                user.Claims.Select(c => new { c.Type, c.Value }));
            return false;
        }

        // Get trainer associated with the user
        var trainer = await trainerService.GetTrainerByUserIdAsync(userId);

        if (trainer == null)
        {
            logger.LogWarning("Authorization failed - No trainer found for User ID: {UserId}", userId);
            return false;
        }

        var hasAccess = trainer.TrainerId == trainerId;
        logger.LogInformation(
            "Authorization {Result} - User ID: {UserId}, Trainer ID: {TrainerID}, Target Trainer ID: {TargetTrainerId}",
            hasAccess ? "granted" : "denied",
            userId,
            trainer.TrainerId,
            trainerId);

        return hasAccess;
    }
}