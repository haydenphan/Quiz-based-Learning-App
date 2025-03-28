using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace QuizWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterAsync(email, password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginAsync(email, password);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View();
        }

        // GET: Login with Google
        [HttpGet]
        public async Task<IActionResult> LoginWithGoogle(string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Account", new { returnUrl });
            var properties = await _accountService.GetGoogleAuthPropertiesAsync(redirectUrl);
            return Challenge(properties, "Google");
        }

        // Google Callback
        [HttpGet]
        public async Task<IActionResult> GoogleCallback(string returnUrl = null)
        {
            return await _accountService.HandleGoogleCallbackAsync(returnUrl);
        }

        // POST: Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}