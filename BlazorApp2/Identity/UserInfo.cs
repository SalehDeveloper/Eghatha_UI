using System.Security.Claims;

namespace BlazorApp2.Identity
{
    public record UserInfo(string UserId, string Email, IList<string> Roles, IList<Claim> Claims);
}