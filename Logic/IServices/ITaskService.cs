using Diagramma_Ganta.Dto.Task;

namespace Diagramma_Ganta.Logic.IServices;

public interface ITaskService
{
    public Task<Guid?> CreateTask(Guid projId, string title, Guid token);
    public Task<bool> DeleteTask(Guid taskId);
    public Task<bool> UpdateTask(Guid taskId, UpdateTaskDto updateDto);
    public Task<List<TaskDto>> GetProjectTasks(Guid projectId);
    public Task<TaskDto?> GetTaskById(Guid taskId);
    public Task<List<TaskPredecessorDto>> GetAvailablePredecessors(Guid projectId, Guid? currentTaskId = null);
}