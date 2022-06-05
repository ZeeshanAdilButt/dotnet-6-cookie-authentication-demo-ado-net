using CookiesAuthenticationPOC.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CookiesAuthenticationPOC.Controllers
{
    public class AccountController : Controller
    {
        private HttpContext? httpContext;

        public IAccountInfrastructure AccountInfrastructure { get; }

        public AccountController(IHttpContextAccessor httpContext, IAccountInfrastructure accountInfrastructure)
        {
            this.httpContext = httpContext.HttpContext;
            AccountInfrastructure = accountInfrastructure;
        }


        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await httpContext.SignOutAsync();

            return RedirectToAction("Index","Home");
        }


        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] User userp, [FromQuery] string redirect)
        {
            var user = new User { Email= userp.Email, Password= userp.Password }; // AuthenticateUser(Input.Email, Input.Password);

            if (!await AccountInfrastructure.IsAuthenticated(user))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                TempData["LoginError"] = "Invalid login attempt.";
                return View("Login");
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("FullName", user.Email),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = userp.RememberMe,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                RedirectUri = redirect //'"/Auth/Login"
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

            return RedirectToAction("Index","Home");

        }
    }
}
