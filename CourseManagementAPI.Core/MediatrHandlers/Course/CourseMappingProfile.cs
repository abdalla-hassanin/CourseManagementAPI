using AutoMapper;
using CourseManagementAPI.Core.MediatrHandlers.Course.Commands;

namespace CourseManagementAPI.Core.MediatrHandlers.Course;

public class CourseMappingProfile : Profile
{
    public CourseMappingProfile()
    {
        CreateMap<Data.Entities.Course, CourseDto>();
        CreateMap<CreateCourseCommand, Data.Entities.Course>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(_ => Ulid.NewUlid().ToString()));
        CreateMap<UpdateCourseCommand, Data.Entities.Course>();
    }
}