using Azure.Core;
using Diagramma_Ganta.Context;
using Diagramma_Ganta.Dto;
using Diagramma_Ganta.Logic.IServices;
using Microsoft.EntityFrameworkCore;

namespace Diagramma_Ganta.Logic.Services;

public class TeamService : ITeamService
{
    private readonly AppDbContext _db;

    public TeamService(AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<bool?> AddUser(Guid projectId, string email, Guid sessionToken)
    {

        var team = await _db.Teams.Include(t=>t.Users).FirstOrDefaultAsync(t => t.ProjectId == projectId);
        var user = await _db.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        
        var session = await _db.Sessions.FirstOrDefaultAsync(s=>s.Token == sessionToken && s.ExpiresAt > DateTime.UtcNow);
        if (session == null)
            return null;
        
        var isOwner = await _db.Projects.Include(p => p.Owner).Where(p => p.OwnerId == session.UserId && p.Id == projectId).AnyAsync();
        if (user != null && team != null && isOwner)
        {
            if (team.Users.Any(u => u.Id == user.Id)) 
                return false;
        
            team.Users.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }

        return null;
    }

    public async Task<bool?> DeleteUser(Guid projectId,Guid userId,Guid sessionToken)
    {
        var team = await _db.Teams.Include(t=>t.Users).FirstOrDefaultAsync(t => t.ProjectId == projectId);
        var user = await _db.Users.Where(u => u.Id==userId).FirstOrDefaultAsync();
        
        
        var session = await _db.Sessions.FirstOrDefaultAsync(s=>s.Token == sessionToken && s.ExpiresAt > DateTime.UtcNow);
        if (session == null)
            return null;
        
        var isOwner = await _db.Projects.Include(p => p.Owner)
            .Where(p => p.OwnerId == session.UserId &&
                        p.Id == projectId && userId!=p.OwnerId).AnyAsync();
        
        if (user != null && team != null && isOwner)
        {
            if (!team.Users.Contains(user))
                return false;
        
            team.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }
        return null;
    }

    public async Task<List<UserDtoOut>?> GetUsers(Guid projId)
    {
        var team = await _db.Teams.Include(t=>t.Users).FirstOrDefaultAsync(t=>t.ProjectId == projId);
        if (team == null)
            return null;
        
        var users = team.Users.Select(u => new UserDtoOut
        {
            Id = u.Id,
            Email = u.Email
        }).ToList();
        
        return users;
    }
}           