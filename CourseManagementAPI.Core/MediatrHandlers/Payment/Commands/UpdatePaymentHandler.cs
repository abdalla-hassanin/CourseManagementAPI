using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Payment.Commands;

public record UpdatePaymentCommand(
    string PaymentId,
    string TrainerId,
    string CourseId,
    decimal Amount,
    DateTime PaymentDate
) : IRequest<ApiResponse<PaymentDto>>;

public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
{
    public UpdatePaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("Payment ID is required.")
            .Length(26).WithMessage("Payment ID must be a valid ULID (26 characters).")
            .Must(id => Ulid.TryParse(id, out _)).WithMessage("Payment ID must be a valid ULID format.");
        RuleFor(x => x.TrainerId)
            .NotEmpty().WithMessage("Trainer ID is required.")
            .Length(26).WithMessage("Trainer ID must be a valid ULID (26 characters).")
            .Must(id => Ulid.TryParse(id, out _)).WithMessage("Trainer ID must be a valid ULID format.");
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Course ID is required.")
            .Length(26).WithMessage("Course ID must be a valid ULID (26 characters).")
            .Must(id => Ulid.TryParse(id, out _)).WithMessage("Course ID must be a valid ULID format.");
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");
        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("Payment date is required.");
    }
}

public class UpdatePaymentHandler(
    IPaymentService paymentService,
    IMapper mapper,
    ILogger<UpdatePaymentHandler> logger) : IRequestHandler<UpdatePaymentCommand, ApiResponse<PaymentDto>>
{
    public async Task<ApiResponse<PaymentDto>> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating payment with ID: {PaymentId}", request.PaymentId);
        try
        {
            var existingPayment = await paymentService.GetPaymentByIdAsync(request.PaymentId, cancellationToken);
            if (existingPayment is null)
            {
                logger.LogWarning("Payment with ID: {PaymentId} not found", request.PaymentId);
                return ApiResponse<PaymentDto>.Factory.NotFound("Payment not found");
            }

            mapper.Map(request, existingPayment);
            var updatedPayment = await paymentService.UpdatePaymentAsync(existingPayment, cancellationToken);
            logger.LogInformation("Payment with ID: {PaymentId} updated successfully", request.PaymentId);
            return ApiResponse<PaymentDto>.Factory.Success(mapper.Map<PaymentDto>(updatedPayment));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating payment with ID: {PaymentId}", request.PaymentId);
            throw;
        }
    }
}