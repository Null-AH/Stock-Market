using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.Comment;
using api.Helpers;
using api.Models;
namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<CommentDto>?> GetAllAsync(QueryObjectComment queryObjectComment);
        Task<CommentDto?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment?> UpdateAsync(Comment comment, int id);
        Task<Comment?> DeleteAsync(int id);
        Task<bool> CommentExist(int id);

    }
}