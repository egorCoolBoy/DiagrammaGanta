using System.Globalization;
using System.Net;
using Diagramma_Ganta.Dto.Project;
using Diagramma_Ganta.Logic.IServices;
using Diagramma_Ganta.Context;
using Diagramma_Ganta.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = Diagramma_Ganta.Model.Task;

namespace Diagramma_Ganta.Logic.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _db;

    public ProjectService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<List<GetProjectDto?>> GetProjects(Guid sessionToken)
    {
        var session = await _db.Sessions.FirstOrDefaultAsync(s=>s.Token == sessionToken && s.ExpiresAt > DateTime.UtcNow);
        if (session == null)
            return null;
        
        var projects = await _db.Projects.Include(p=>p.Owner)
            .Include(p=>p.Subject).Where(p => p.Team.Users.Any(u => u.Id == session.UserId))
            .Select(p => new GetProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Subject = p.Subject.Title,
                CreationDate = p.CreationDate,
                Description = p.Description,
                IsOwner = session.UserId == p.OwnerId,
                OwnerName = p.Owner.Email
            }).ToListAsync();
        return projects;
    }

    public async Task<GetProjectDto> GetProjectById(Guid projId,Guid sessionToken)
    {
        
        var session = await _db.Sessions.FirstOrDefaultAsync(s=>s.Token == sessionToken && s.ExpiresAt > DateTime.UtcNow);
        if (session == null)
            return null;
        
        var proj = await _db.Projects.Include(p=>p.Owner).Include(p=>p.Subject).Select(p=> new GetProjectDto
        {
            Id = p.Id,
            Title = p.Title,
            Subject = p.Subject.Title,
            CreationDate = p.CreationDate,
            Description = p.Description,
            IsOwner = p.OwnerId == session.UserId,
            OwnerName = p.Owner.Email
        }).FirstOrDefaultAsync(p=>p.Id == projId);
        return proj;
    }
    public async Task<Guid?> CreateProject([FromQuery]CreateProjectDto proj,Guid token)
    {
        var session =await  _db.Sessions.Include(s => s.User)
            .Where(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync(); 
        
        if (session == null)
            return null;
        
        
        var newProject = new Project
        {
            Id = Guid.NewGuid(),
            Title = proj.Title,
            CreationDate = DateTime.UtcNow,
            Description = proj.Description,
            SubjectId = proj.SubjectId,
            OwnerId = session.UserId,
            Owner = session.User,
        };
        
        await _db.Projects.AddAsync(newProject);
        await _db.SaveChangesAsync(); 
        
        var team = new Team
        {
            ProjectId = newProject.Id,
            Users = new List<User>()
        };
        
        team.Users.Add(session.User);
    
        await _db.Teams.AddAsync(team);
        newProject.Team = team;
        await _db.SaveChangesAsync();
    
        return newProject.Id;
    }
    
    public async Task<bool> Delete(Guid projId,Guid token)
    {
        var session =await  _db.Sessions.Include(s => s.User)
            .Where(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();
        
        if (session == null)
            return false;
        var isOwner = await _db.Projects.Include(p => p.Owner).Where(p => p.OwnerId == session.UserId && p.Id == projId).AnyAsync();
        if (!isOwner)
            return false;
        
        
        var desc = await _db.Projects.FirstOrDefaultAsync(p=>p.Id == projId);
        _db.Projects.Remove(desc);
        await _db.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> Update(UpdateDescriptionDto desc)
    {
        var proj = await _db.Projects.Where(p => p.Id == desc.ProjectId).FirstOrDefaultAsync();
        //if (proj!=null)
            proj.Description = desc.Text;
        await _db.SaveChangesAsync();
        return true;
    }
}