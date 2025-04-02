using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


using api.Models;
using api.Data;
using api.Interfaces;
using api.Dtos.Stock;
using Microsoft.CodeAnalysis.CSharp;
using api.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;


namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _ApplicationDBContext;
        public StockRepository(ApplicationDBContext context)
        {
            _ApplicationDBContext = context;
        }
        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {
            var stocks = _ApplicationDBContext.Stock.Include(c => c.Comments).ThenInclude(a=> a.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if(!string.IsNullOrWhiteSpace(query.SortBt))
            {
                if (query.IsSortAscending)
                {
                    stocks = stocks.OrderBy(s => EF.Property<object>(s, query.SortBt));
                }
                else
                {
                    stocks = stocks.OrderByDescending(s => EF.Property<object>(s, query.SortBt));
                }
            }
            else
            {
                stocks = stocks.OrderBy(s => s.Id);
            }

            var skip = (query.PageNumber - 1) * query.PageSize;
            return await stocks.Skip(skip).Take(query.PageSize).ToListAsync();

        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _ApplicationDBContext.Stock
                .Include(s => s.Comments)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            await _ApplicationDBContext.Stock.AddAsync(stockModel);
            await _ApplicationDBContext.SaveChangesAsync();
            return stockModel;
        }
        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _ApplicationDBContext.Stock.FindAsync(id);
            if (existingStock == null)
            {
                return null;
            }
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Symbol = stockDto.Symbol;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;

            await _ApplicationDBContext.SaveChangesAsync();
            return existingStock;
        }
        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var stock = await _ApplicationDBContext.Stock.FindAsync(id);
            if (stock == null)
            {
                return null;
            }
            _ApplicationDBContext.Stock.Remove(stock);
            await _ApplicationDBContext.SaveChangesAsync();
            return stock;
        }

        public Task<bool> StockExists(int id)
        {
            return _ApplicationDBContext.Stock.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _ApplicationDBContext.Stock.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        
    }
}
