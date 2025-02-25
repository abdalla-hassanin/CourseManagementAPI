using AutoMapper;
using CourseManagementAPI.Core.MediatrHandlers.Admin.Commands;

namespace CourseManagementAPI.Core.MediatrHandlers.Admin;

public class AdminMappingProfile : Profile
{
    public AdminMappingProfile()
    {
        CreateMap<Data.Entities.Admin, AdminDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.ApplicationUser.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.ApplicationUser.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email));
        CreateMap<UpdateAdminCommand, Data.Entities.Admin>();
    }
}