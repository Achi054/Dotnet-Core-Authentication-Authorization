using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthorization.Authorization
{
    public class CustomClaim : IAuthorizationRequirement
    {
        public CustomClaim(string claim)
        {
            UserClaim = claim;
        }

        public string UserClaim { get; }
    }

    public class AdminClaimHandler : AuthorizationHandler<CustomClaim>
    {
        protected override Task HandleRequirementAsync
            (AuthorizationHandlerContext context, CustomClaim requirement)
        {
            var isValidClaim = context.User.Claims.Any(x => x.Type == requirement.UserClaim);

            return Task.CompletedTask;
        }
    }
}
