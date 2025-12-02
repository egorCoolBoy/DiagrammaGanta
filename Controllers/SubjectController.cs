using Diagramma_Ganta.Logic.IServices;
using Diagramma_Ganta.Model;
using Microsoft.AspNetCore.Mvc;

namespace Diagramma_Ganta.Controllers;

public class SubjectController : ControllerBase
{
    private readonly ISubjectsService _subjectsServiceService;

    public SubjectController(ISubjectsService projServiceService)
    {
        _subjectsServiceService = projServiceService;
    }
    
    
    [HttpGet("get/subjects")]
    public async Task<IActionResult> Get()
    {
        
        var subjects = await _subjectsServiceService.GetSubjects();

        return Ok(subjects);
    }
    
}