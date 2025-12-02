using Diagramma_Ganta.Context;
using Diagramma_Ganta.Dto.Subject;
using Diagramma_Ganta.Logic.IServices;
using Diagramma_Ganta.Model;
using Microsoft.EntityFrameworkCore;

namespace Diagramma_Ganta.Logic.Services;

public class SubjectsServiceService : ISubjectsService
{
    private readonly AppDbContext _db;

    public SubjectsServiceService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<SubjectDto>> GetSubjects()
    {
        var subjects = await _db.Subjects.Select(s => new SubjectDto
        {
            Id = s.Id,
            Title = s.Title
            
        }).ToListAsync();
        return subjects;
    }
}