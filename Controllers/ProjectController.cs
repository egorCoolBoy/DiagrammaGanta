using Diagramma_Ganta.Dto.Project;
using Diagramma_Ganta.Logic.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diagramma_Ganta.Controllers;

[ApiController]
[Route("api")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projService;

    public ProjectController(IProjectService projService)
    {
        _projService = projService;
    }
    
    [HttpGet("get/projects")]
    public async Task<IActionResult> Get()
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Unauthorized(new{message = "Не авторизованный пользователь"});
    
        var projects = await _projService.GetProjects(Guid.Parse(token));
    
        if (projects == null)
            return Unauthorized(new{message = "Не авторизованный пользователь"});
    
        return Ok(projects);
    }

    [HttpGet("get/project{id}")]
    public async Task<IActionResult> GetProject(Guid id)
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Unauthorized();
        var project = await  _projService.GetProjectById(id,Guid.Parse(token));
        return Ok(project);
    }

    [HttpPost("post/project")]
    public async Task<IActionResult> Post([FromQuery] CreateProjectDto proj)
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Unauthorized();
        var responce = await _projService.CreateProject(proj,Guid.Parse(token));
        return Created("",new {Id = responce});
    }
    
    [HttpDelete("delete/project/{projectId}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Unauthorized();
        
        if (await _projService.Delete(projectId,Guid.Parse(token)))
            return Ok();
        return BadRequest(new{message = "что то пошло не так"});
    }

    [HttpPatch("patch/description")]
    public async Task<IActionResult> UpdateDescription([FromQuery] UpdateDescriptionDto description)
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Unauthorized();
        if (await _projService.Update(description))
            return Ok();
        return BadRequest();
    }
    


}