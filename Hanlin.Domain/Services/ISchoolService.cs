using Hanlin.Domain.Models;

namespace Hanlin.Domain.Services
{
    public interface ISchoolService
    {
        Term GetCurrentTerm();
        string GetBaselineRocYear();
        Curriculum GetCurrentCurriculum(string educationCode, string subjectCode, string headerPart41);
    }
}
