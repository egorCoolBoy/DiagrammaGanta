using Diagramma_Ganta.Context;
using Diagramma_Ganta.Dto.Task;
using Diagramma_Ganta.Logic.IServices;
using Diagramma_Ganta.Model;
using Microsoft.EntityFrameworkCore;
using Task = Diagramma_Ganta.Model.Task;
using TaskStatus = Diagramma_Ganta.Model.TaskStatus;

namespace Diagramma_Ganta.Logic.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid?> CreateTask(Guid projId, string title,Guid token)
    {
        var project =  await _db.Projects.FirstOrDefaultAsync(p=> p.Id == projId);
        var session =  await _db.Sessions.FirstOrDefaultAsync(s=>s.Token == token && s.ExpiresAt > DateTime.UtcNow);
        
        
        if (session == null || project == null) 
            return null;
        
        var task = new Task
        {
            Id = Guid.NewGuid(),
            Title = title,
            ProjectId = projId,
            AuthorTaskId = session.UserId,
            CreationDate = DateTime.UtcNow,
            TaskStatus = TaskStatus.None
        };
        
        await _db.Tasks.AddAsync(task);
        await _db.SaveChangesAsync();
        return task.Id;

    }

    public async Task<List<TaskDto>> GetProjectTasks(Guid projectId)
    {
        return await _db.Tasks
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Users) 
            .Include(t => t.Dependencies) 
            .ThenInclude(d => d.Predecessor) 
            .Include(t => t.AuthorTask) 
            .Select(t => new TaskDto
            {
                Id = t.Id,
                CreationDate = t.CreationDate,
                Title = t.Title,
                Description = t.Description,
                TaskStatus = t.TaskStatus,
                Deadline = t.Deadline,
                Author = new UserDtoOut 
                { 
                    Id = t.AuthorTask.Id, 
                    Email = t.AuthorTask.Email 
                },
                Users = t.Users.Select(u => new UserDtoOut 
                { 
                    Id = u.Id, 
                    Email = u.Email 
                }).ToList(),
                Predecessors = t.Dependencies.Select(d => new TaskPredecessorDto
                {
                    Id = d.Predecessor.Id,
                    Title = d.Predecessor.Title
                }).ToList()
            }).OrderBy(t=>t.CreationDate)
            .ToListAsync();
    }    
    
        
    public async Task<bool> DeleteTask(Guid taskId)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        
        if (task == null)
            return false;
        
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> UpdateTask(Guid taskId, UpdateTaskDto updateDto)
    {
        var task = await _db.Tasks
            .Include(t => t.Users)
            .Include(t => t.Dependencies)
            .FirstOrDefaultAsync(t => t.Id == taskId);

        if (task == null)
            return false;
        
        if (updateDto.Title != null)
            task.Title = updateDto.Title;

        if (updateDto.Description != null)
            task.Description = updateDto.Description;

        if (updateDto.TaskStatus.HasValue)
            task.TaskStatus = (TaskStatus)updateDto.TaskStatus.Value;

        if (updateDto.Deadline.HasValue)
            task.Deadline = updateDto.Deadline.Value;
        
        if (updateDto.Users != null)
        {
            var team = await _db.Teams
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.ProjectId == task.ProjectId);
        
            if (team == null)
                return false; 

            var userIds = updateDto.Users.Select(u => u.Id).ToList();
            
            var validUserIds = team.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => u.Id)
                .ToList();
            
            if (validUserIds.Count != userIds.Count)
                return false;

            var users = await _db.Users
                .Where(u => validUserIds.Contains(u.Id))
                .ToListAsync();
    
            task.Users = users;
        }
    
        if (updateDto.Predecessors != null)
        {
            var validPredecessorIds = await _db.Tasks
                .Where(t => t.ProjectId == task.ProjectId && updateDto.Predecessors.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync();

            
            if (validPredecessorIds.Count != updateDto.Predecessors.Count)
                return false;

            _db.TaskDependencies.RemoveRange(task.Dependencies);
        
            var newDependencies = validPredecessorIds
                .Select(predecessorId => new TaskDependency
                {
                    TaskId = taskId,
                    PredecessorId = predecessorId
                })
                .ToList();

            await _db.TaskDependencies.AddRangeAsync(newDependencies);
            task.Dependencies = newDependencies;
        }

        await _db.SaveChangesAsync();
        return true;
    }
    
    
    public async Task<TaskDto?> GetTaskById(Guid taskId)
    {
        return await _db.Tasks
            .Where(t => t.Id == taskId)
            .Include(t => t.Users) 
            .Include(t => t.Dependencies) 
            .ThenInclude(d => d.Predecessor) 
            .Include(t => t.AuthorTask) 
            .Select(t => new TaskDto
            {
                Id = t.Id,
                CreationDate = t.CreationDate,
                Title = t.Title,
                Description = t.Description,
                TaskStatus = t.TaskStatus,
                Deadline = t.Deadline,
                Author = new UserDtoOut { Id = t.AuthorTask.Id, Email = t.AuthorTask.Email },
                Users = t.Users.Select(u => new UserDtoOut { Id = u.Id, Email = u.Email }).ToList(),
                Predecessors = t.Dependencies.Select(d => new TaskPredecessorDto
                {
                    Id = d.Predecessor.Id,
                    Title = d.Predecessor.Title
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }
    
    
    public async Task<List<TaskPredecessorDto>> GetAvailablePredecessors(Guid projectId, Guid? currentTaskId = null)
    {
        var query = _db.Tasks
            .Where(t => t.ProjectId == projectId);
        
        if (currentTaskId.HasValue)
        {
            query = query.Where(t => t.Id != currentTaskId.Value);
        }
    
        return await query
            .Select(t => new TaskPredecessorDto
            {
                Id = t.Id,
                Title = t.Title
            })
            .ToListAsync();
    }
    
    
}