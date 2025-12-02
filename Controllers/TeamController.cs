using Diagramma_Ganta.Dto.Team;
using Diagramma_Ganta.Logic.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Diagramma_Ganta.Controllers;

[ApiController]
[Route("api")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService ts)
    {
        _teamService = ts;
    }


    [HttpPatch("add/user")]
    public async Task<IActionResult> AddUser(AddUserInTeamDto body)
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

        var sessionToken = Request.Cookies["session"];
        if (sessionToken == null)
            return Unauthorized();
        var response = await _teamService.AddUser(body.ProjectId, body.Email, Guid.Parse(sessionToken));

        if (response == false)
            return Conflict(new {message = $"{body.Email} уже есть в команде" });

        if (response == null)
            return BadRequest(new { message = $"{body.Email} еще не зарегестировался " });

        return Ok(new { message = $"{body.Email} добавлен" });
    }

    [HttpDelete("delete/user")]
    public async Task<IActionResult> Deleteuser(DeleteUserDto body)
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

        var sessionToken = Request.Cookies["session"];
        if (sessionToken == null)
            return Unauthorized();


        var response = await _teamService.DeleteUser(body.ProjectId, body.UserId, Guid.Parse(sessionToken));

        if (response == false)
            return Conflict(new { message = "Что то пошло не так" });

        if (response == null)
            return Conflict(new { message = $"Нет такого пользователя" });

        return Ok();

    }


    [HttpGet("get/users")]
    public async Task<IActionResult> GetUsers([FromQuery]Guid projectId)
    {
        var users = await _teamService.GetUsers(projectId);
        if (users == null)
            return NotFound(new{message = "нет такого проекта"});
        return Ok(users);
    }







}