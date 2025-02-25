using CourseManagementAPI.Core.Base.Response;
using CourseManagementAPI.Core.MediatrHandlers.Course;
using CourseManagementAPI.Core.MediatrHandlers.Payment;
using CourseManagementAPI.Core.MediatrHandlers.Trainer;
using Swashbuckle.AspNetCore.Filters;

namespace CourseManagementAPI.Api.ResponseExample;

public class GetTrainerByIdResponseExample : IExamplesProvider<ApiResponse<TrainerDto>>
{
    public ApiResponse<TrainerDto> GetExamples()
    {
        var trainerDto = new TrainerDto
        {
            TrainerId = "01HF3WFKX1KPY89WNJRXJ6V18N",
            ApplicationUserId = "01HF3WFKX1KPY89WNJRXJ6V18N",
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Bio = "Experienced trainer with 5 years in course management."
        };

        return ApiResponse<TrainerDto>.Factory.Success(trainerDto);
    }
}

public class GetAllTrainersResponseExample : IExamplesProvider<ApiResponse<IReadOnlyList<TrainerDto>>>
{
    public ApiResponse<IReadOnlyList<TrainerDto>> GetExamples()
    {
        var trainers = new List<TrainerDto>
        {
            new TrainerDto
            {
                TrainerId = "01HF3WFKX1KPY89WNJRXJ6V18N",
                ApplicationUserId = "01HF3WFKX1KPY89WNJRXJ6V18N",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Bio = "Experienced trainer with 5 years in course management."
            },
            new TrainerDto
            {
                TrainerId = "01HF3WFKX1KPY89WNJRXJ6V18M",
                ApplicationUserId = "01HF3WFKX1KPY89WNJRXJ6V18M",
                FirstName = "Mark",
                LastName = "Johnson",
                Email = "mark.johnson@example.com",
                Bio = "Specialist in technical training."
            }
        };

        return ApiResponse<IReadOnlyList<TrainerDto>>.Factory.Success(trainers);
    }
}

public class UpdateTrainerResponseExample : IExamplesProvider<ApiResponse<TrainerDto>>
{
    public ApiResponse<TrainerDto> GetExamples()
    {
        var trainerDto = new TrainerDto
        {
            TrainerId = "01HF3WFKX1KPY89WNJRXJ6V18N",
            ApplicationUserId = "01HF3WFKX1KPY89WNJRXJ6V18N",
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Bio = "Updated bio: Expert trainer with advanced certifications."
        };

        return ApiResponse<TrainerDto>.Factory.Success(trainerDto, "Trainer updated successfully");
    }
}

public class DeleteTrainerResponseExample : IExamplesProvider<ApiResponse<bool>>
{
    public ApiResponse<bool> GetExamples()
    {
        return ApiResponse<bool>.Factory.Success(true, "Trainer deleted successfully");
    }
}

public class GetCoursesForTrainerResponseExample : IExamplesProvider<ApiResponse<IReadOnlyList<CourseDto>>>
{
    public ApiResponse<IReadOnlyList<CourseDto>> GetExamples()
    {
        var courses = new List<CourseDto>
        {
            new CourseDto("01HF3WFKX1KPY89WNJRXJ6V18N", "Course 1", "Description 1", DateTime.UtcNow.AddDays(5), DateTime.UtcNow.AddDays(10), 199.99m, 20, 30, "01HF3WFKX1KPY89WNJRXJ6V18N"),
            new CourseDto("01HF3WFKX1KPY89WNJRXJ6V18M", "Course 2", "Description 2", DateTime.UtcNow.AddDays(15), DateTime.UtcNow.AddDays(20), 299.99m, 30, 25, "01HF3WFKX1KPY89WNJRXJ6V18N")
        };

        return ApiResponse<IReadOnlyList<CourseDto>>.Factory.Success(courses, "Courses retrieved successfully");
    }
}

public class GetPaymentsForTrainerResponseExample : IExamplesProvider<ApiResponse<IReadOnlyList<PaymentDto>>>
{
    public ApiResponse<IReadOnlyList<PaymentDto>> GetExamples()
    {
        var payments = new List<PaymentDto>
        {
            new PaymentDto("01HF3WFKX1KPY89WNJRXJ6V18N", "01HF3WFKX1KPY89WNJRXJ6V18N", "01HF3WFKX1KPY89WNJRXJ6V18M", 199.99m, DateTime.UtcNow.AddDays(-5)),
            new PaymentDto("01HF3WFKX1KPY89WNJRXJ6V18P", "01HF3WFKX1KPY89WNJRXJ6V18N", "01HF3WFKX1KPY89WNJRXJ6V18Q", 299.99m, DateTime.UtcNow.AddDays(-2))
        };

        return ApiResponse<IReadOnlyList<PaymentDto>>.Factory.Success(payments, "Payments retrieved successfully");
    }
}
public class BadRequestTrainerResponseExample : IExamplesProvider<ApiResponse<TrainerDto>>
{
    public ApiResponse<TrainerDto> GetExamples()
    {
        return ApiResponse<TrainerDto>.Factory.BadRequest(
            "Invalid trainer data",
            new List<string> { "ApplicationUserId is required" }
        );
    }
}

public class UnauthorizedTrainerResponseExample : IExamplesProvider<ApiResponse<TrainerDto>>
{
    public ApiResponse<TrainerDto> GetExamples()
    {
        return ApiResponse<TrainerDto>.Factory.Unauthorized("Unauthorized access");
    }
}

public class NotFoundTrainerResponseExample : IExamplesProvider<ApiResponse<TrainerDto>>
{
    public ApiResponse<TrainerDto> GetExamples()
    {
        return ApiResponse<TrainerDto>.Factory.NotFound("Trainer not found");
    }
}