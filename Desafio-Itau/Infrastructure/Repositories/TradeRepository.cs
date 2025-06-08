using DesafioInvestimentosItau.Application.User.User.Client;
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
            .Include(t => t.Asset)
            .Where(t => t.UserId == userId &&
                        t.Type == TradeTypeEnum.Buy &&
                        t.Asset.Code == assetCode)
            .ToListAsync();
    }
    
    public async Task<List<TradeEntity>> GetGroupedBuyTradesByUserAsync(long userId)
    {
        return await _context.Trades
            .Include(t => t.Asset)
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
}