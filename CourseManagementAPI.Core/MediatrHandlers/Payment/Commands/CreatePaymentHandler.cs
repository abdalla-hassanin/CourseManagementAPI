using AutoMapper;
using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Service.IService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseManagementAPI.Core.MediatrHandlers.Payment.Commands;

public record CreatePaymentCommand(
    string TrainerId,
    string CourseId,
    decimal Amount,
    DateTime PaymentDate
) : IRequest<ApiResponse<PaymentDto>>;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
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

public class CreatePaymentHandler(
    IPaymentService paymentService,
    IMapper mapper,
    ILogger<CreatePaymentHandler> logger) : IRequestHandler<CreatePaymentCommand, ApiResponse<PaymentDto>>
{
    public async Task<ApiResponse<PaymentDto>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new payment for TrainerId: {TrainerId}, CourseId: {CourseId}", request.TrainerId, request.CourseId);
        try
        {
            var payment = mapper.Map<Data.Entities.Payment>(request);
            var createdPayment = await paymentService.CreatePaymentAsync(payment, cancellationToken);
            logger.LogInformation("Payment created successfully with ID: {PaymentId}", createdPayment.PaymentId);
            return ApiResponse<PaymentDto>.Factory.Created(mapper.Map<PaymentDto>(createdPayment));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating payment for TrainerId: {TrainerId}", request.TrainerId);
            throw;
        }
    }
}