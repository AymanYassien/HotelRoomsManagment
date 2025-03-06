using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using hotel_room_api;
using Hotel_Rooms_MVC.Models;
using Hotel_Rooms_MVC.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using RoomsUtility;

namespace Hotel_Rooms_MVC.Controllers;

public class AuthController : Controller
{
    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginRequestDTO());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task <IActionResult> Login(LoginRequestDTO loginRequestDto, string returnUrl = null)
    {
        APIResponse response = await _authService.LoginAsync<APIResponse>(loginRequestDto);
        if (response != null && response.IsSuccess)
        {
            LoginResponseDTO loginResponseDto =
                JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginResponseDto.Token);
            
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            
            identity.AddClaim(new Claim(ClaimTypes.Name, loginResponseDto.User.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type =="role" ).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            HttpContext.Session.SetString(StaticData.SessionToken, loginResponseDto.Token);
            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect("http://localhost:5042" + returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("CustomError", response?.ErrorMessages.FirstOrDefault() ?? "Login failed.");
        return View(loginRequestDto);
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task <IActionResult> Register(RegisterRequestDTO registerRequestDto, string returnUrl = null)
    {
        APIResponse registerResponse = await _authService.RegisterAsync<APIResponse>(registerRequestDto);
        if (registerResponse !=  null && registerResponse.IsSuccess)
        {
            LoginRequestDTO loginRequestDto = new LoginRequestDTO()
            { UserName = registerRequestDto.UserName, Password = registerRequestDto.Password };
            
            APIResponse loginResponse = await _authService.LoginAsync<APIResponse>(loginRequestDto);
            if (loginResponse != null && loginResponse.IsSuccess)
            {
                
                LoginResponseDTO loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(loginResponse.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(loginResponseDto.Token);
                
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            
                identity.AddClaim(new Claim(ClaimTypes.Name, loginResponseDto.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type =="role" ).Value));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                
                
                HttpContext.Session.SetString(StaticData.SessionToken, loginResponseDto.Token);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            
            ModelState.AddModelError("CustomError", "Login failed after registration. Please try logging in manually.");
            return View(registerRequestDto);
        }

        
        ModelState.AddModelError("CustomError", registerResponse?.ErrorMessages.FirstOrDefault() ?? "Registration failed.");
        return View(registerRequestDto);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.SetString(StaticData.SessionToken, "");

        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }
}