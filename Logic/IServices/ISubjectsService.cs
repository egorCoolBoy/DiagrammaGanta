using Diagramma_Ganta.Dto.Subject;

namespace Diagramma_Ganta.Logic.IServices;

public interface ISubjectsService
{
    public Task<List<SubjectDto>> GetSubjects();
}