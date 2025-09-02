using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using api.DTO.Comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using api.Mappers;
using Microsoft.AspNetCore.Identity;
using api.Extension;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/Comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IFMPService _fmpService;
        public CommentController(UserManager<AppUser> userManager, ICommentRepository commentRepo,
        IStockRepository stockRepo, IFMPService fmpService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fmpService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] QueryObjectComment qoComment)
        {
            var comments = await _commentRepo.GetAllAsync(qoComment);
            if (comments == null) return NotFound();
            if (!comments.Any()) return NoContent();
            return Ok(comments);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (!await _commentRepo.CommentExist(id)) return NotFound();
            return Ok(comment);
        }

        [HttpPost("{symbol:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromRoute] string symbol, [FromBody] CreateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var stock = await _stockRepo.GetStockBySymbol(symbol);
            if (stock == null)
            {
                await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock does not exists!");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var commentModel = commentDto.ToCommentFromCreateRequestDto(stock.Id);
            commentModel.AppUserId = appUser.Id;

            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.CommentToDto());
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequestDto commentDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _commentRepo.CommentExist(id)) return BadRequest();
            var commentModel = await _commentRepo.UpdateAsync(commentDto.ToCommentFromUpdateRequestDto(), id);
            if (commentModel == null) return NotFound();
            return Ok(commentModel.CommentToDto());
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _commentRepo.CommentExist(id)) return BadRequest();
            var commentModel = await _commentRepo.DeleteAsync(id);
            if (commentModel == null) return NotFound();
            return NoContent();
        }
        

    }
}