using AutoMapper;
using CourseManagementAPI.Core.MediatrHandlers.Trainer.Commands;

namespace CourseManagementAPI.Core.MediatrHandlers.Trainer;

public class TrainerMappingProfile : Profile
{
    public TrainerMappingProfile()
    {
        CreateMap<Data.Entities.Trainer, TrainerDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.ApplicationUser.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.ApplicationUser.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));


        CreateMap<UpdateTrainerCommand, Data.Entities.Trainer>();
    }
}