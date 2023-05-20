
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using WebMud.Models;

namespace WebMud.Data
{
    [Route("/[controller]")]
    [ApiController]
    public class CookieController : Controller
    {
        private readonly IUsersService _usersService;



        public CookieController(IUsersService usersService)
        {
            _usersService = usersService;
        }



        [HttpPost]
        public async Task<ActionResult> Login([FromForm] UserCredentials credentials)
        {

            //bruteforce delay
            if (string.IsNullOrEmpty(credentials.Username) == false || string.IsNullOrEmpty(credentials.Password) == false)
            {
                await Task.Delay(5000);

            }


            List<Users> Users = new List<Users>();
            Users = await _usersService.GetUsers();


            bool isremembered = false;
            if (credentials.IsRemembered == "on")
            {
                isremembered = true;
            }

            if (Users.Any(a => a.Email.Equals(credentials.Username) && a.Password.Equals(credentials.Password) && a.IsVerified.Equals(true) && a.IsAuthenticated.Equals(true) && a.IsCustomer.Equals(false)))
            {



                var authUser = Users.First(a => a.Email.Equals(credentials.Username) && a.Password.Equals(credentials.Password));

                //tohle by chtělo předělat na podrobnější systém rolí

                string role = "User";
                if (authUser.IsAdmin)
                {
                    role = "Admin";
                }

                //


                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, authUser.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, authUser.Email));
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
                identity.AddClaim(new Claim(ClaimTypes.Email, authUser.Email));

                var principal = new ClaimsPrincipal(identity);


                //Nastavení vlastností cookies, dle toho jestli user dá "pamatovat si mě"...
                AuthenticationProperties authProperties;
                if (!isremembered)
                {
                    authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.Now.AddMinutes(60),
                        IsPersistent = true,
                        RedirectUri = this.Request.Host.Value,

                    };
                }
                else
                {
                    authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.Now.AddDays(30),
                        IsPersistent = true,
                        RedirectUri = this.Request.Host.Value,

                    };
                }


                //Přidá počet návštěvností
                authUser.TrafficCount = authUser.TrafficCount + 1;
                await _usersService.UpdateUsers(authUser);

                //Přidělení Cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);


                return LocalRedirect("/personal/dashboard");

            }
            else
                return LocalRedirect("/pages/authentication/login/Invalid credentials!");

        }

        //[HttpGet("/account/logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    Console.WriteLine("xxxx");
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return LocalRedirect("/pages/authentication/login");
        //}


    }

    public class UserCredentials
    {
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Username { get; set; }
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Password { get; set; }
        public string? IsRemembered { get; set; }
    }
}