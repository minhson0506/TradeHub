using DataAccess.Contexts;
using DataAccess.Models;
using Infrastructure.Interfaces;

namespace Infrastructure
{
    public class ApplicationUserRepository : 
        GenericRepository<ApplicationUser>, 
        IApplicationUserRepository<ApplicationUser>
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public ApplicationUser GetApplicationUserById(string id)
        {
            return _dbContext.ApplicationUsers.Find(id);
        }
    }
}
