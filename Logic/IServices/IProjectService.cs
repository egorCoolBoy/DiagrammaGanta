using Diagramma_Ganta.Dto.Project;

namespace Diagramma_Ganta.Logic.IServices;

public interface IProjectService
{
    public Task<List<GetProjectDto?>> GetProjects(Guid sessionToken);
    public Task<Guid?> CreateProject(CreateProjectDto proj,Guid token);
    public Task<bool> Delete(Guid projId,Guid token);
    public Task<bool> Update(UpdateDescriptionDto desc);
    public Task<GetProjectDto> GetProjectById(Guid id);
}