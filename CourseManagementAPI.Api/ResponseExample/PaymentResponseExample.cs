using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Core.MediatrHandlers.Payment;
using Swashbuckle.AspNetCore.Filters;

namespace CourseManagementAPI.Api.ResponseExample;

public class GetPaymentByIdResponseExample : IExamplesProvider<ApiResponse<PaymentDto>>
{
    public ApiResponse<PaymentDto> GetExamples()
    {
        var paymentDto = new PaymentDto(
            PaymentId: "01HF3WFKX1KPY89WNJRXJ6V18N",
            TrainerId: "01HF3WFKX1KPY89WNJRXJ6V18N",
            CourseId: "01HF3WFKX1KPY89WNJRXJ6V18M",
            Amount: 199.99m,
            PaymentDate: DateTime.UtcNow
        );

        return ApiResponse<PaymentDto>.Factory.Success(paymentDto);
    }
}

public class GetAllPaymentsResponseExample : IExamplesProvider<ApiResponse<IReadOnlyList<PaymentDto>>>
{
    public ApiResponse<IReadOnlyList<PaymentDto>> GetExamples()
    {
        var payments = new List<PaymentDto>
        {
            new PaymentDto("01HF3WFKX1KPY89WNJRXJ6V18N", "01HF3WFKX1KPY89WNJRXJ6V18N", "01HF3WFKX1KPY89WNJRXJ6V18M", 199.99m, DateTime.UtcNow.AddDays(-5)),
            new PaymentDto("01HF3WFKX1KPY89WNJRXJ6V18M", "01HF3WFKX1KPY89WNJRXJ6V18N", "01HF3WFKX1KPY89WNJRXJ6V18P", 299.99m, DateTime.UtcNow.AddDays(-3))
        };

        return ApiResponse<IReadOnlyList<PaymentDto>>.Factory.Success(payments);
    }
}

public class CreatedPaymentResponseExample : IExamplesProvider<ApiResponse<PaymentDto>>
{
    public ApiResponse<PaymentDto> GetExamples()
    {
        var paymentDto = new PaymentDto(
            PaymentId: "01HF3WFKX1KPY89WNJRXJ6V18N",
            TrainerId: "01HF3WFKX1KPY89WNJRXJ6V18N",
            CourseId: "01HF3WFKX1KPY89WNJRXJ6V18M",
            Amount: 149.99m,
            PaymentDate: DateTime.UtcNow
        );

        return ApiResponse<PaymentDto>.Factory.Created(paymentDto, "Payment created successfully");
    }
}

public class UpdatePaymentResponseExample : IExamplesProvider<ApiResponse<PaymentDto>>
{
    public ApiResponse<PaymentDto> GetExamples()
    {
        var paymentDto = new PaymentDto(
            PaymentId: "01HF3WFKX1KPY89WNJRXJ6V18N",
            TrainerId: "01HF3WFKX1KPY89WNJRXJ6V18N",
            CourseId: "01HF3WFKX1KPY89WNJRXJ6V18M",
            Amount: 179.99m,
            PaymentDate: DateTime.UtcNow.AddDays(1)
        );

        return ApiResponse<PaymentDto>.Factory.Success(paymentDto, "Payment updated successfully");
    }
}

public class DeletePaymentResponseExample : IExamplesProvider<ApiResponse<bool>>
{
    public ApiResponse<bool> GetExamples()
    {
        return ApiResponse<bool>.Factory.Success(true, "Payment deleted successfully");
    }
}

public class BadRequestPaymentResponseExample : IExamplesProvider<ApiResponse<PaymentDto>>
{
    public ApiResponse<PaymentDto> GetExamples()
    {
        return ApiResponse<PaymentDto>.Factory.BadRequest(
            "Invalid payment data",
            new List<string> { "Amount must be greater than 0", "TrainerId is required" }
        );
    }
}

public class UnauthorizedPaymentResponseExample : IExamplesProvider<ApiResponse<PaymentDto>>
{
    public ApiResponse<PaymentDto> GetExamples()
    {
        return ApiResponse<PaymentDto>.Factory.Unauthorized("Unauthorized access");
    }
}

public class NotFoundPaymentResponseExample : IExamplesProvider<ApiResponse<PaymentDto>>
{
    public ApiResponse<PaymentDto> GetExamples()
    {
        return ApiResponse<PaymentDto>.Factory.NotFound("Payment not found");
    }
}