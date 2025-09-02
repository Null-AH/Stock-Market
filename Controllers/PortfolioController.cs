using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using api.Extension;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IFMPService _fmpService;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo,
         IPortfolioRepository portfolioRepo, IFMPService fMPService)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
            _fmpService = fMPService;
        }

        [HttpGet]

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);
            if (!userPortfolio.Any()) return NoContent();
            return Ok(userPortfolio);
        }

        [HttpPost]

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(string symbol)
        {

            var username = User.GetUsername();
            var appuser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetStockBySymbol(symbol);

            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest("Stock does not exists!");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }

            if (stock == null) return BadRequest("Stock not found");
            if (!await _stockRepo.StockExist(stock.Id)) return NoContent();

            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appuser);

            if (userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Stock already exist within user portfolio");

            var porfolioModel = new Portfolio
            {
                AppUserId = appuser.Id,
                StockId = stock.Id
            };

            if (porfolioModel == null) return StatusCode(500, "Server Error");

            await _portfolioRepo.CreateAsync(porfolioModel);

            return StatusCode(201, "Stock added succesfully");
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null) return NotFound();

            var stock = await _stockRepo.GetStockBySymbol(symbol);
            if (stock == null) return BadRequest("Stock Not Found");
            if (!await _stockRepo.StockExist(stock.Id)) return NoContent();

            var userportfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);


            var filteredPorofolio = userportfolio.Where(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase)).ToList();

            if (filteredPorofolio.IsNullOrEmpty())
                return BadRequest("No such stock within this portfolio");
            else
                await _portfolioRepo.DeleteAsync(appUser, symbol);

            return Ok("Stock Deleted from your portfolio!");
 
        }
        

    }
}