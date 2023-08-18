using DataAccess.Contexts;
using DataAccess.Models;
using Infrastructure.Interfaces;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;  //dependency injection of Data Source

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IGenericRepository<ApplicationUser> _ApplicationUser;
        private IGenericRepository<Attachment> _Attachment;
        private IGenericRepository<Comment> _Comment;
        private IGenericRepository<Post> _Post;
        private IGenericRepository<Status> _Status;
        private IGenericRepository<Tag> _Tag;


        //ADD ADDITIONAL MODELS HERE
        public IGenericRepository<ApplicationUser> ApplicationUser
        {
            get
            {

                if (_ApplicationUser == null)
                {
                    _ApplicationUser = new GenericRepository<ApplicationUser>(_dbContext);
                }

                return _ApplicationUser;
            }
        }

        public IGenericRepository<Attachment> Attachment
        {
            get
            {

                if (_Attachment == null)
                {
                    _Attachment = new GenericRepository<Attachment>(_dbContext);
                }

                return _Attachment;
            }
        }

        public IGenericRepository<Comment> Comment
        {
            get
            {

                if (_Comment == null)
                {
                    _Comment = new GenericRepository<Comment>(_dbContext);
                }

                return _Comment;
            }
        }

        public IGenericRepository<Post> Post
        {
            get
            {

                if (_Post == null)
                {
                    _Post = new GenericRepository<Post>(_dbContext);
                }

                return _Post;
            }
        }

        

        public IGenericRepository<Status> Status
        {
            get
            {
                if (_Status == null)
                {
                    _Status = new GenericRepository<Status>(_dbContext);
                }

                return _Status;
            }
        }

        public IGenericRepository<Tag> Tag
        {
            get
            {
                if (_Tag == null)
                {
                    _Tag = new GenericRepository<Tag>(_dbContext);
                }

                return _Tag;
            }
        }
        //ADD ADDITIONAL METHODS FOR EACH MODEL (similar to Category) HERE

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        //additional method added for garbage disposal

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }

}

