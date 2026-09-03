using System.Security.Claims;

namespace PlaylistManagement.Api.Extensions
{
    //to avoid repeating claims logic in every controller
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user identity");
            }

            return userId;
        }
    }
}
