using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDto(this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                Purchase = stockModel.Purchase,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.CommentToDto()).ToList()
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                Purchase = stockDto.Purchase,
                MarketCap = stockDto.MarketCap
            };
        }

        public static Stock ToStockFromFMP(this FMPStock fmpstock)
        {
            return new Stock
            {
                Symbol = fmpstock.symbol,
                CompanyName = fmpstock.companyName,
                LastDiv = (decimal)fmpstock.lastDividend,
                Industry = fmpstock.industry,
                Purchase = (decimal)fmpstock.price,
                MarketCap = fmpstock.marketCap
            };
        }
    }
}