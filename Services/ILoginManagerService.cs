using System.Threading.Tasks;
using learner_portal.Models;

namespace learner_portal.Services
{
    public interface ILoginManagerService
    {
        Task<Users> GetCurrentUser();
        Task<Users> GetUserRoles(string userName);
        Task<Company> GetCurrentCompany();
        Task<Learner> GetCurrentLeaner();
        Task<bool> SetCurrentUser(string userName);

    }
}