using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet()]

        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult>GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _stockRepo.GetAllAsync(query);
            if (stocks == null) return NotFound("Stock Do NOT exist in the database");
            if (!stocks.Any()) return StatusCode(204,"Stock Do NOT exist in the database");

            return Ok(stocks);
        }

        [HttpGet("{id:int}")]

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStockById(int id)
        {
            var stock = await _stockRepo.GetStockByIdAsync(id);

            if (stock == null) return NotFound($"Stock with ID {id} do not exist");

            return Ok(stock.ToStockDto());
        }

        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNewStock([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var stockModel = await _stockRepo.CreateAsync(stockDto);

            return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]

        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockRequestDto stockDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var stockModel = await _stockRepo.UpdateAsync(id, stockDto);
            if (stockModel == null) return NotFound();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var stockModel = await _stockRepo.DeleteAsync(id);

            if (stockModel == null) return NotFound();

            return NoContent();
        }

    }
}