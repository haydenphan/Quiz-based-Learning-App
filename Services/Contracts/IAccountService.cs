using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Services.Contracts
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(string email, string password);
        Task<Microsoft.AspNetCore.Identity.SignInResult> LoginAsync(string email, string password);
        Task<AuthenticationProperties> GetGoogleAuthPropertiesAsync(string redirectUrl);
        Task<IActionResult> HandleGoogleCallbackAsync(string returnUrl);
        Task LogoutAsync();
    }
}
