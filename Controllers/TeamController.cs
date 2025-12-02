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
            return Conflict(new { User = "already exists in team" });

        if (response == null)
            return BadRequest(new { message = $"no {body.Email} in base or you have not rights" });

        return Ok(new { message = $"{body.Email} added" });
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
            return Conflict(new { message = "UnExpected" });

        if (response == null)
            return Conflict(new { message = "No user or team in base" });

        return Ok();

    }


    [HttpGet("get/users")]
    public async Task<IActionResult> GetUsers([FromQuery]Guid projectId)
    {
        var users = await _teamService.GetUsers(projectId);
        if (users == null)
            return NotFound(new{message = "no project"});
        return Ok(users);
    }







}