using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using api.DTO.Comment;
using api.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using api.Helpers;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }


        public async Task<List<CommentDto>?> GetAllAsync(QueryObjectComment queryObjectComment)
        {
            var comments = _context.Comments.Include(a => a.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObjectComment.Symbol)) comments = comments.Where(c => c.Stock.Symbol.ToLower() == queryObjectComment.Symbol.ToLower());
            if (!string.IsNullOrWhiteSpace(queryObjectComment.SortBy))
            {
                if (queryObjectComment.SortBy.Equals("CreatedOn", StringComparison.OrdinalIgnoreCase))
                {
                    comments = queryObjectComment.IsDescending ? comments.OrderByDescending(c => c.CreatedOn) : comments.OrderBy(c => c.CreatedOn);
                }
            }

            var skipNumer = (queryObjectComment.PageNumber - 1) * queryObjectComment.PageSize;

            var commentsDto = comments.Select(c => c.CommentToDto());
            
            if (commentsDto == null) return null;

            return await commentsDto.Skip(skipNumer).Take(queryObjectComment.PageSize).ToListAsync();
        }

        public async Task<CommentDto?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null) return null;
            return comment.CommentToDto();
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        public async Task<Comment?> UpdateAsync(Comment comment, int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (commentModel == null) return null;
            commentModel.Title = comment.Title;
            commentModel.Content = comment.Content;
            await _context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<bool> CommentExist(int id)
        {
            return await _context.Comments.AnyAsync(c => c.Id == id);
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (commentModel == null) return null;
            _context.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }
    }
}