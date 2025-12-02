using Diagramma_Ganta.Dto.Task;
using Diagramma_Ganta.Logic.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Diagramma_Ganta.Controllers;

[ApiController]
[Route("api")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost("post/task")]
    public async Task<IActionResult> CreateTask(Guid projectId, string title)
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Conflict(new { message = "Нет сессии" });
        if (title == null)
            return BadRequest();

        var taskId = await _taskService.CreateTask(projectId, title, Guid.Parse(token));

        return Ok(taskId);
    }

    [HttpDelete("delete/task")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Unauthorized();
        if (await _taskService.DeleteTask(taskId))
            return Ok();
        return BadRequest();
    }


    [HttpPut("put/task")]
    public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskDto updateDto)
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Unauthorized();
        if (updateDto == null)
            return BadRequest(new { message = "Нечего обновлять" });
        
        if (updateDto.Title == null && 
            updateDto.Description == null && 
            !updateDto.TaskStatus.HasValue && 
            !updateDto.Deadline.HasValue && 
            updateDto.Users == null && 
            updateDto.Predecessors == null)
        {
            return BadRequest(new { message = "Нечего обновлять" });
        }

        var result = await _taskService.UpdateTask(taskId, updateDto);
    
        if (!result)
            return NotFound(new { message = "Задача не найдена" });

        return Ok(new { message = "Задача успешно обновлена" });
    }
    
    [HttpGet("project/{projectId}/tasks")]
    public async Task<IActionResult> GetProjectTasks(Guid projectId)
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Unauthorized();
        var tasks = await _taskService.GetProjectTasks(projectId);
        return Ok(tasks);
    }
    
    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetTaskById(Guid taskId)
    {
        var token = Request.Cookies["session"];
        if (token == null)
            return Unauthorized();
        var task = await _taskService.GetTaskById(taskId);
        if (task == null)
            return NotFound(new { message = "Задача не найдена" });
        return Ok(task);
    }
    
    
    [HttpGet("project/{projectId}/predecessors")]
    public async Task<IActionResult> GetAvailablePredecessors(Guid projectId, [FromQuery] Guid? currentTaskId = null)
    {
        var predecessors = await _taskService.GetAvailablePredecessors(projectId, currentTaskId);
        return Ok(predecessors);
    }

}