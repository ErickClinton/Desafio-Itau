using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using DesafioInvestimentosItau.Domain.Enums;
using DesafioInvestimentosItau.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioInvestimentosItau.Infrastructure.Repositories;

public class TradeRepository : ITradeRepository
{
    private readonly ApplicationDbContext _context;

    public TradeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TradeEntity> CreateAsync(TradeEntity trade)
    {
        _context.Trades.Add(trade);
        await _context.SaveChangesAsync();
        return trade;
    }
    public async Task<List<TradeEntity>> GetBuyTradesByUserAndAssetAsync(long userId, string assetCode)
    {
        return await _context.Trades
            .Include(t => t.AssetCode)
            .Where(t => t.UserId == userId &&
                        t.Type == TradeTypeEnum.Buy &&
                        t.AssetCode == assetCode)
            .ToListAsync();
    }
    
    public async Task<List<TradeEntity>> GetGroupedBuyTradesByUserAsync(long userId)
    {
        return await _context.Trades
            .Where(t => t.UserId == userId && t.Type == TradeTypeEnum.Buy)
            .ToListAsync();
    }
    public async Task<decimal> GetTotalInvestedAsync(long userId)
    {
        return await _context.Trades.Where(t => t.UserId == userId && t.Type == TradeTypeEnum.Buy)
            .SumAsync(t => t.UnitPrice * t.Quantity + t.BrokerageFee);
    }

    public async Task<decimal> GetTotalBrokerageFeeAsync(long userId)
    {
        return await _context.Trades
            .Where(t => t.UserId == userId)
            .SumAsync(t => t.BrokerageFee);
    }
    
    public async Task<decimal> GetTotalBrokerageAsync()
    {
        return await _context.Trades.SumAsync(t => t.BrokerageFee);
    }
    
    public async Task<List<TopBrokerageDto>> GetTopUserBrokeragesAsync(int top)
    {
        var topUserIds = await _context.Trades
            .GroupBy(t => t.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                TotalBrokerage = g.Sum(t => t.BrokerageFee)
            })
            .OrderByDescending(x => x.TotalBrokerage)
            .Take(top)
            .Select(x => x.UserId)
            .ToListAsync();

        var result = await _context.Trades
            .GroupBy(t => new { t.UserId, t.User.Name,t.User.Email })
            .Select(g => new TopBrokerageDto()
            {
                UserId = g.Key.UserId,
                UserName = g.Key.Name,
                Email = g.Key.Email,
                TotalBrokerage = g.Sum(t => t.BrokerageFee)
            })
            .OrderByDescending(g => g.TotalBrokerage)
            .Take(10)
            .ToListAsync();
        return result;
    }
    
    public async Task<List<TradeEntity>> GetBuyTradesByAssetAsync(string assetCode)
    {
        return await _context.Trades
            .Where(t => t.Type == TradeTypeEnum.Buy && t.AssetCode == assetCode)
            .ToListAsync();
    }
}