using System;
using DataAccess.Models;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<ApplicationUser> ApplicationUser { get; }
        public IGenericRepository<Attachment> Attachment { get; }
        public IGenericRepository<Comment> Comment { get; }
        public IGenericRepository<Post> Post { get; }
        public IGenericRepository<Status> Status { get; }
        public IGenericRepository<Tag> Tag { get; }

        //ADD other Models/Tables here as you create them

        //save changes to the data source

        int Commit();

        Task<int> CommitAsync();

    }

}

