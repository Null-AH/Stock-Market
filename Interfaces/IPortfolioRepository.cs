using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        public Task<List<Stock>> GetUserPortfolioAsync(AppUser user);
        public Task<Portfolio> CreateAsync(Portfolio portfolio);
        public Task<Portfolio> DeleteAsync(AppUser appUser,string symbol);
    }
}