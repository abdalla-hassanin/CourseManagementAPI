using CourseManagementAPI.Api.Base;
using CourseManagementAPI.Api.ResponseExample;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Core.MediatrHandlers.Payment;
using CourseManagementAPI.Core.MediatrHandlers.Payment.Commands;
using CourseManagementAPI.Core.MediatrHandlers.Payment.Queries;
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
public class PaymentController(IMediator mediator, ILogger<PaymentController> logger) : ControllerBase
{
    [HttpGet("{paymentId}")]
    [Authorize(Roles = "Admin,Trainer")]
    [SwaggerOperation(
        Summary = "Get Payment by ID",
        Description = "This endpoint allows Admins and Trainers to retrieve a payment by its ID."
    )]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetPaymentByIdResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundPaymentResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedPaymentResponseExample))]
    public async Task<IResult> GetPaymentById(string paymentId, CancellationToken cancellationToken)
    {
        var query = new GetPaymentByIdQuery(paymentId);
        var result = await mediator.Send(query, cancellationToken);
        return result.ToResult();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(
        Summary = "Get All Payments",
        Description = "This endpoint allows Admins to retrieve all payments."
    )]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PaymentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<PaymentDto>>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetAllPaymentsResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedPaymentResponseExample))]
    public async Task<IResult> GetAllPayments(CancellationToken cancellationToken)
    {
        var query = new GetAllPaymentsQuery();
        var result = await mediator.Send(query, cancellationToken);
        return result.ToResult();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(
        Summary = "Create New Payment",
        Description = "This endpoint allows Admins to create a new payment."
    )]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreatedPaymentResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestPaymentResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedPaymentResponseExample))]
    public async Task<IResult> CreatePayment([FromBody] CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.ToResult();
    }

    [HttpPut("{paymentId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(
        Summary = "Update Payment",
        Description = "This endpoint allows Admins to update a payment."
    )]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UpdatePaymentResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundPaymentResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestPaymentResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedPaymentResponseExample))]
    public async Task<IResult> UpdatePayment(string paymentId, [FromBody] UpdatePaymentCommand command, CancellationToken cancellationToken)
    {
        if (paymentId != command.PaymentId)
        {
            return ApiResponseResults.BadRequest("Payment ID mismatch");
        }

        var result = await mediator.Send(command, cancellationToken);
        return result.ToResult();
    }

    [HttpDelete("{paymentId}")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(
        Summary = "Delete Payment",
        Description = "This endpoint allows Admins to delete a payment."
    )]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeletePaymentResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedPaymentResponseExample))]
    public async Task<IResult> DeletePayment(string paymentId, CancellationToken cancellationToken)
    {
        var command = new DeletePaymentCommand(paymentId);
        var result = await mediator.Send(command, cancellationToken);
        return result.ToResult();
    }
}