using CourseManagementAPI.Api.Base;
using CourseManagementAPI.Api.ResponseExample;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Core.MediatrHandlers.Course;
using CourseManagementAPI.Core.MediatrHandlers.Payment;
using CourseManagementAPI.Core.MediatrHandlers.Trainer;
using CourseManagementAPI.Core.MediatrHandlers.Trainer.Commands;
using CourseManagementAPI.Core.MediatrHandlers.Trainer.Queries;
using CourseManagementAPI.Service.IService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrainerController(IMediator mediator, ITrainerService trainerService, ILogger<TrainerController> logger) : ControllerBase
{
    [HttpGet("{trainerId}")]
    [Authorize(Roles = "Admin,Trainer")]
    [SwaggerOperation(
        Summary = "Get Trainer by ID",
        Description = "This endpoint allows Admins and Trainers to retrieve a trainer by its ID."
    )]
    [ProducesResponseType(typeof(ApiResponse<TrainerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TrainerDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<TrainerDto>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetTrainerByIdResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundTrainerResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedTrainerResponseExample))]
    public async Task<IResult> GetTrainerById(
        string trainerId,
        CancellationToken cancellationToken)
    {
        var query = new GetTrainerByIdQuery(trainerId);
        var result = await mediator.Send(query, cancellationToken);
        var canAccess = await AuthorizationHelper.CanAccess(User, result.Data!.TrainerId, trainerService, logger);
        if (canAccess == false)
        {
            return ApiResponseResults.Unauthorized("Can access only own account");
        }

        return result.ToResult();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(
        Summary = "Get All Trainers",
        Description = "This endpoint allows Admins to retrieve all trainers."
    )]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TrainerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TrainerDto>>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllTrainersResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedTrainerResponseExample))]
    public async Task<IResult> GetAllTrainers(CancellationToken cancellationToken)
    {
        var query = new GetAllTrainersQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.ToResult();
    }

    [HttpPut("{trainerId}")]
    [Authorize(Roles = "Admin,Trainer")]
    [SwaggerOperation(
        Summary = "Update Trainer",
        Description = "This endpoint allows Admins and Trainers to update a trainer."
    )]
    [ProducesResponseType(typeof(ApiResponse<TrainerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TrainerDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<TrainerDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<TrainerDto>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UpdateTrainerResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundTrainerResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestTrainerResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedTrainerResponseExample))]
    public async Task<IResult> UpdateTrainer(
        string trainerId,
        [FromBody] UpdateTrainerCommand command,
        CancellationToken cancellationToken)
    {
        if (trainerId != command.TrainerId)
        {
            return ApiResponseResults.BadRequest("Trainer ID mismatch");
        }

        var canAccess = await AuthorizationHelper.CanAccess(User, trainerId, trainerService, logger);
        if (!canAccess)
        {
            return ApiResponseResults.Unauthorized("Can update only own account");
        }

        var result = await mediator.Send(command, cancellationToken);
        return result.ToResult();
    }

    [HttpDelete("{trainerId}")]
    [Authorize(Roles = "Admin,Trainer")]
    [SwaggerOperation(
        Summary = "Delete Trainer",
        Description = "This endpoint allows Admins and Trainers to delete a trainer."
    )]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeleteTrainerResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedTrainerResponseExample))]
    public async Task<IResult> DeleteTrainer(
        string trainerId,
        CancellationToken cancellationToken)
    {
        var canAccess = await AuthorizationHelper.CanAccess(User, trainerId, trainerService, logger);
        if (!canAccess)
        {
            return ApiResponseResults.Unauthorized("Can delete only own account");
        }

        var command = new DeleteTrainerCommand(trainerId);
        var result = await mediator.Send(command, cancellationToken);
        return result.ToResult();
    }
[HttpGet("{trainerId}/courses")]
    [Authorize(Roles = "Admin,Trainer")]
    [SwaggerOperation(
        Summary = "Get All Courses for a Trainer",
        Description = "This endpoint allows Admins and Trainers to retrieve all courses for a specific trainer."
    )]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CourseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CourseDto>>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetCoursesForTrainerResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedTrainerResponseExample))]
    public async Task<IResult> GetCoursesForTrainer(string trainerId, CancellationToken cancellationToken)
    {
        var canAccess = await AuthorizationHelper.CanAccess(User, trainerId, trainerService, logger);
        if (!canAccess)
        {
            return ApiResponseResults.Unauthorized("Can access only own courses");
        }

        var query = new GetCoursesForTrainerQuery(trainerId);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToResult();
    }

    [HttpGet("{trainerId}/payments")]
    [Authorize(Roles = "Admin,Trainer")]
    [SwaggerOperation(
        Summary = "Get All Payments for a Trainer",
        Description = "This endpoint allows Admins and Trainers to retrieve all payments for a specific trainer."
    )]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PaymentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PaymentDto>>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetPaymentsForTrainerResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedTrainerResponseExample))]
    public async Task<IResult> GetPaymentsForTrainer(string trainerId, CancellationToken cancellationToken)
    {
        var canAccess = await AuthorizationHelper.CanAccess(User, trainerId, trainerService, logger);
        if (!canAccess)
        {
            return ApiResponseResults.Unauthorized("Can access only own payments");
        }

        var query = new GetPaymentsForTrainerQuery(trainerId);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToResult();
    }
}