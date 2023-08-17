using DataAccess.Models;

namespace Infrastructure.Interfaces
{
    public interface IApplicationUserRepository<T> : IGenericRepository<ApplicationUser>
    {
        ApplicationUser GetApplicationUserById(string id);
    }
}

