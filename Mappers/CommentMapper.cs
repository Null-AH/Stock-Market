using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto CommentToDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId,
                CreatedBy = comment.AppUser.UserName
            };
        }
        public static Comment ToCommentFromCreateRequestDto(this CreateCommentRequestDto commentDto, int stockId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }
        public static Comment ToCommentFromUpdateRequestDto(this UpdateCommentRequestDto commentDto)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
            };
        }
        
    }
}