using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace api.DTO.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be 5 letter at least")]
        [MaxLength(150,ErrorMessage = "Title cannot be over 50 letters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Comment content must be 5 letter at least")]
        [MaxLength(250,ErrorMessage = "Comment content cannot be over 250 letters")]
        public string Content { get; set; } = string.Empty;
    }
}