using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using GCCorePSAV.Models;
using GCCorePSAV.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GCCorePSAV.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            clsQuery consul = new clsQuery();
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                if (consul._loginSession(model.Email, model.Password))
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Email, model.Email));
                    claims.Add(new Claim(ClaimTypes.Name, model.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin"));
                    ClaimsPrincipal principal = new ClaimsPrincipal();
                    ClaimsIdentity iden = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    principal.AddIdentity(iden);
                    //await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties
                    //{
                    //    IsPersistent = true,
                    //    AllowRefresh=true
                    //});
                    byte[] SessionUSer = System.Text.Encoding.UTF8.GetBytes(model.Email);

                    ViewBag.IsAuth = true;
                    ViewBag.Role = "Admin";
                    Sec.secjwt JWT = new Sec.secjwt();
                    string jwt = JWT.GetToken(model);
                    Response.Cookies.Append("Bearer", jwt, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    Response.Cookies.Append("pandoraRules", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.Email)), new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    Response.Cookies.Append("IsAuth", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.Email)), new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    return RedirectToAction("Index", "PSAV");
                }
                else
                {
                    ViewBag.result = "Credenciales inválidas";
                }
            }
            else
            {
                ViewBag.result = "Error al llenar el formulario";
            }
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Bearer");
            Response.Cookies.Delete("pandoraRules");
            Response.Cookies.Delete("IsAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}
