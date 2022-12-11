using MicroSocialPlatform.Models;

namespace MicroSocialPlatform.Misc;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// This class derives from SignInManager and implements the login based
// on email address rather than on username.
public class MySignInManager : SignInManager<AppUser>
{
    public MySignInManager(
        Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<AppUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<AppUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<AppUser> confirmation) :
        base(userManager, contextAccessor, claimsFactory, optionsAccessor,
            logger, schemes, confirmation)
    {
    }

    public override async Task<SignInResult> PasswordSignInAsync(
        string userName,
        string password,
        bool isPersistent,
        bool lockoutOnFailure)
    {
        // Find by email, not by username.
        AppUser byNameAsync = await this.UserManager.FindByEmailAsync(userName);
        return (object)byNameAsync == null
            ? SignInResult.Failed
            : await this.PasswordSignInAsync(byNameAsync, password, isPersistent, lockoutOnFailure);
    }
}