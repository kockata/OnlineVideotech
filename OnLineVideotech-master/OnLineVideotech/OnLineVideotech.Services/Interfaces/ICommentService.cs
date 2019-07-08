using OnLineVideotech.Data.Models;
using OnLineVideotech.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnLineVideotech.Services.Interfaces
{
    public interface ICommentService : IBaseService
    {
        Task<List<CommentServiceModel>> GetAllCommentsForMovie(Guid movieId);

        Task AddCommentForMovie(string comment, string userId, Guid movieId);

        Task DeleteComment(Guid id);

        Task<CommentServiceModel> FindComment(Guid id);
    }
}
