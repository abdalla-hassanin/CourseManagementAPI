using AutoMapper;
using CourseManagementAPI.Service.Base;

namespace CourseManagementAPI.Core.MediatrHandlers.Auth;

public class AuthMappingProfile: Profile
{
    public AuthMappingProfile()
    {
        CreateMap<AuthResult, AuthDto>();
    }
}
