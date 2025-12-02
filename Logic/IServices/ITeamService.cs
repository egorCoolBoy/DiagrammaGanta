using Diagramma_Ganta.Dto;

namespace Diagramma_Ganta.Logic.IServices;

public interface ITeamService
{
    public  Task<bool?> AddUser(Guid projectId, string email,Guid currentUserId);
    public Task<bool?> DeleteUser(Guid userId,Guid projectId,Guid currentUserId);
    public Task<List<UserDtoOut>?> GetUsers(Guid userId);
}