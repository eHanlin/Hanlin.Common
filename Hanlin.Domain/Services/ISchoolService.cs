using Hanlin.Domain.Models;

namespace Hanlin.Domain.Services
{
    public interface ISchoolService
    {
        Term GetCurrentTerm();
        string GetBaselineRocYear();
    }
}
