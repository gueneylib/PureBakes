namespace PureBakes.Services;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PureBakes.Service.Services.Interface;

public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    public string GetUserId()
    {
        var principalUser = httpContextAccessor.HttpContext?.User;
        return principalUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    public string GetUserRole()
    {
        var principalUser = httpContextAccessor.HttpContext?.User;
        return principalUser?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
    }
}