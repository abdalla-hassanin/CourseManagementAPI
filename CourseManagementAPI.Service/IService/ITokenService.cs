using System.Security.Claims;

namespace CourseManagementAPI.Service.IService;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    int GetAccessTokenExpirationMinutes();

}