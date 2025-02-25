using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Data.Entities;
using CourseManagementAPI.Service.IService;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Auth.Commands;

public record RegisterTrainerCommand(
    string Username,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Bio
) : IRequest<ApiResponse<bool>>;

public class RegisterTrainerCommandValidator : AbstractValidator<RegisterTrainerCommand>
{
    public RegisterTrainerCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Bio).NotEmpty().MaximumLength(500).WithMessage("Bio must not exceed 500 characters.");
    }
}

public class RegisterTrainerCommandHandler(
    IAuthService authService,
    ITrainerService trainerService,
    UserManager<ApplicationUser> userManager,
    ILogger<RegisterTrainerCommandHandler> logger)
    : IRequestHandler<RegisterTrainerCommand, ApiResponse<bool>>
{
    public async Task<ApiResponse<bool>> Handle(RegisterTrainerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to register new trainer with username {Username}", request.Username);

        try
        {
            var existingEmail = await userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                logger.LogWarning("Email {Email} is already registered", request.Email);
                return ApiResponse<bool>.Factory.BadRequest("This email is already registered");
            }

            var existingUsername = await userManager.FindByNameAsync(request.Username);
            if (existingUsername != null)
            {
                logger.LogWarning("Username {Username} is already taken", request.Username);
                return ApiResponse<bool>.Factory.BadRequest("This username is already taken");
            }

            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            var authResult = await authService.RegisterUserAsync(user, request.Password, "Trainer", cancellationToken);

            if (!authResult.Succeeded)
            {
                logger.LogWarning("Trainer registration failed for username {Username}. Reason: {Message}",
                    request.Username, authResult.Message);
                return ApiResponse<bool>.Factory.BadRequest(authResult.Message ?? "Registration failed");
            }

            var createdUser = await userManager.FindByNameAsync(request.Username);
            if (createdUser == null)
            {
                logger.LogWarning("User creation failed for username {Username}", request.Username);
                return ApiResponse<bool>.Factory.BadRequest("User creation failed");
            }

            var trainer = new Data.Entities.Trainer
            {
                ApplicationUserId = createdUser.Id,
                Bio = request.Bio,
            };

            await trainerService.CreateTrainerAsync(trainer, cancellationToken);

            logger.LogInformation("Trainer registered successfully with username {Username}", request.Username);
            return ApiResponse<bool>.Factory.Created(true, authResult.Message ?? "Trainer registered successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during trainer registration for username {Username}",
                request.Username);
            throw;
        }
    }
}