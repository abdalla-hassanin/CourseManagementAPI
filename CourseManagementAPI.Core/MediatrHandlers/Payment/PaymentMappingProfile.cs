using AutoMapper;
using CourseManagementAPI.Core.MediatrHandlers.Payment.Commands;

namespace CourseManagementAPI.Core.MediatrHandlers.Payment;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<Data.Entities.Payment, PaymentDto>();
        CreateMap<CreatePaymentCommand, Data.Entities.Payment>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(_ => Ulid.NewUlid().ToString()));
        CreateMap<UpdatePaymentCommand, Data.Entities.Payment>();
    }
}