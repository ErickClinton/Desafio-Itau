using DesafioInvestimentosItau.Application.User.User.Client;
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
    
    public async Task<decimal> GetTotalInvestedAsync(long userId)
    {
        
        return await _context.Trades.Where(t => t.UserId == userId && t.Type == TradeTypeEnum.Buy)
            .SumAsync(t => t.UnitPrice * t.Quantity);
    }

    public async Task<decimal> GetTotalBrokerageFeeAsync(long userId)
    {
        return await _context.Trades
            .Where(t => t.UserId == userId)
            .SumAsync(t => t.BrokerageFee);
    }
}