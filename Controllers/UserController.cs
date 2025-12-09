using Diagramma_Ganta.Logic.IServices;
using Diagramma_Ganta.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Diagramma_Ganta.Controllers;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{

    private readonly IUserService _autoService;

    public UserController(IUserService autoservice)
    {
        _autoService = autoservice;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Post(RegistRequest user)
    {
        if (!ModelState.IsValid)
        {
            var errors = new Dictionary<string, string>();
            foreach (var entry in ModelState)
            {
                if (entry.Value.ValidationState == ModelValidationState.Invalid)
                {
                    var message = entry.Value.Errors[0].ErrorMessage;
                    var fieldName = entry.Key;
                    errors[fieldName] = message;
                }
            }
            return BadRequest(errors);
        }
        
        if (!await _autoService.IsEmailUnique(user.email))
        {
            return Conflict(new { Email = "Такой email уже существует" });
        }

        var userId = await _autoService.Register(user);
        return Created("", new { message = "Вы успешно зарегестрировались" });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var tokenCheck = HttpContext.Request.Cookies["session"];

         if (tokenCheck != null)
             return Conflict(new{message = "Вы уже залогинены, сначала разлогиньтесь"});
        
        var token = await _autoService.Login(request);
    
        if (token == null)
            return Unauthorized(new { message = "невалидная почта или пароль" });
        
        var expires = DateTime.UtcNow.AddDays(1);
        Response.Cookies.Append("session", token, new CookieOptions
        {
            HttpOnly = true,
            Path = "/",
            Expires = expires,
            Secure = true,    
            SameSite = SameSiteMode.None
        });

        return Ok(new { message = "Вы успешно вошли в систему" });
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var sessionToken = Request.Cookies["session"];
    
        if (string.IsNullOrEmpty(sessionToken))
        {
            return Unauthorized(new { message = "Сессия неактуальна" });
        }
        
        var result = await _autoService.Logout(sessionToken);
    
        if (result == null)
        {
            return Unauthorized(new { message = "Невалидный куки" });
        }
        
        Response.Cookies.Delete("session");

        return Ok(new { message = "Успешно" });
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var sessionToken = Request.Cookies["session"];
    
        if (string.IsNullOrEmpty(sessionToken))
            return Unauthorized(new { message = false });

        var user = await _autoService.GetUser(sessionToken);
    
        if (user == null)
            return Unauthorized(new { message = false });


        return Ok(user);
    }
    
}