using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnboardingAzureB2CCustomInvite.Authz;

public class IsAdminHandlerUsingAzureGroups : AuthorizationHandler<IsAdminRequirement>
{
    private readonly string _adminGroupId;

    public IsAdminHandlerUsingAzureGroups(IConfiguration configuration)
    {
        _adminGroupId = configuration.GetValue<string>("AzureGroups:AdminGroupId");
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        if (requirement == null)
            throw new ArgumentNullException(nameof(requirement));

        var claimIdentityprovider = context.User.Claims.FirstOrDefault(t => t.Type == "group"
            && t.Value == _adminGroupId);

        if (claimIdentityprovider != null)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}