using Final_project.CustomPolicyProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Final_project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy = "Claim.har")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [Authorize(Roles = "designer")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

        [SecurityLevel(5)]
        public IActionResult SecretLevel()
        {
            return View("Secret");
        }

        [SecurityLevel(10)]
        public IActionResult SecretHigherLevel()
        {
            return View("Secret");
        }

        [AllowAnonymous]
        public IActionResult Authenticate()
        {
            var webClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "harminder"),
                new Claim(ClaimTypes.Email, "harminder@fmail.com"),
                new Claim(ClaimTypes.DateOfBirth, "10/10/1998"),
                new Claim(ClaimTypes.Role, "designer"),
                new Claim(ClaimTypes.Role, "designerTwo"),
                new Claim(DynamicPolicies.SecurityLevel, "7"),
                new Claim("webpage.Says", "everything look good"),
            };

            var securityClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "harminder"),
                new Claim("securityLicense", "A+"),
            };

            var webIdentity = new ClaimsIdentity(webClaims, "web Identity");
            var securityIdentity = new ClaimsIdentity(securityClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] { webIdentity, securityIdentity });
            
            //-----------------------------------------------------------
            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DoStuff(
            [FromServices] IAuthorizationService authorizationService)
        {
            // we are doing stuff here

            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();

            var authResult = await authorizationService.AuthorizeAsync(User, customPolicy);

            if (authResult.Succeeded)
            {
                return View("Index");
            }

            return View("Index");
        }

    }
}
