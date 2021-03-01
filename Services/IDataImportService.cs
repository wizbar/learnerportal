using System.Threading.Tasks;

namespace learner_portal.Services
{
    public interface IDataImportService
    {
        public Task<bool> ImportExcelForLearners(string fileName);
    }
}