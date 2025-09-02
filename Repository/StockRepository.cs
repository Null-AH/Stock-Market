using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.Stock;
using api.Mappers;
using api.Interfaces;
using api.Models;
using api.Data;
using Microsoft.EntityFrameworkCore;
using api.Helpers;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<StockDTO>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName)) stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            if (!string.IsNullOrWhiteSpace(query.Symbol)) stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
                if (query.SortBy.Equals("Lastdiv", StringComparison.OrdinalIgnoreCase)) {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.LastDiv) : stocks.OrderBy(s => s.LastDiv);                    
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            var stocksDto = stocks.Select(s => s.ToStockDto());

            return await stocksDto.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            var stock = await _context.Stocks.Include(c => c.Comments).ThenInclude(a => a.AppUser).FirstOrDefaultAsync(stock => stock.Id == id);
            if (stock == null) return null;
            return stock;
        }

        public async Task<Stock> CreateAsync(CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

            if (stockModel == null) return null;

            stockModel.Symbol = stockDto.Symbol;
            stockModel.CompanyName = stockDto.CompanyName;
            stockModel.LastDiv = stockDto.LastDiv;
            stockModel.Industry = stockDto.Industry;
            stockModel.Purchase = stockDto.Purchase;
            stockModel.MarketCap = stockDto.MarketCap;
            await _context.SaveChangesAsync();

            return stockModel;
        }
        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

            if (stockModel == null) return null;
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }
        public async Task<bool> StockExist(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetStockBySymbol(string symbol)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol.ToLower() == symbol.ToLower());

            if (stock == null) return null;
            return stock;
        }
    }
}