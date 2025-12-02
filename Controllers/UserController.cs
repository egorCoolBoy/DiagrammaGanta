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
            return Conflict(new { Email = "already exists" });
        }

        var userId = await _autoService.Register(user);
        return Created("", new { id = userId });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto request)
    {
        var token = await _autoService.Login(request);
    
        if (token == null)
            return Unauthorized(new { message = "Invalid email or password" });
        
        var expires = DateTime.UtcNow.AddDays(30);
        Response.Cookies.Append("session", token, new CookieOptions
        {
            HttpOnly = true,
            Path = "/",
            Expires = expires,
            Secure = true,    
        });

        return Ok(new { message = "true" });
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var sessionToken = Request.Cookies["session"];
    
        if (string.IsNullOrEmpty(sessionToken))
        {
            return Unauthorized(new { message = "No session cookie" });
        }
        
        var result = await _autoService.Logout(sessionToken);
    
        if (result == null)
        {
            return Unauthorized(new { message = "Invalid cookie" });
        }
        
        Response.Cookies.Delete("session");

        return Ok(new { message = "successful" });
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var sessionToken = Request.Cookies["session"];
    
        if (string.IsNullOrEmpty(sessionToken))
            return Unauthorized(new { message = "No session cookie" });

        var user = await _autoService.GetUser(sessionToken);
    
        if (user == null)
            return Unauthorized(new { message = "No session cookie" });


        return Ok(user);
    }
    
}