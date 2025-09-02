using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<StockDTO>> GetAllAsync(QueryObject query);
        Task<Stock?> GetStockByIdAsync(int id);
        Task<Stock> CreateAsync(CreateStockRequestDto stockDto);
         Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExist(int id);
        Task<Stock?> GetStockBySymbol(string symbol);
    }
}