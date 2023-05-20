using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WebMud.Data
{
    public class LogOutCookieController: Controller
    {
        [HttpGet("/pages/authentication/logout")]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           
            return LocalRedirect("/");
        }



    }
}
