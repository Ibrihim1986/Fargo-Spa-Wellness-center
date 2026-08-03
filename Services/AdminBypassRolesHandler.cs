using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace Family_and_Spa_Wellness.Services;

public class AdminBypassRolesHandler : AuthorizationHandler<RolesAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
    {
        if (context.User.HasClaim(ClaimTypes.Role, "Admin"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
