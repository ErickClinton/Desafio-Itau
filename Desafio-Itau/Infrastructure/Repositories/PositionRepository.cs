using DesafioInvestimentosItau.Application.User.User.Client;
using DesafioInvestimentosItau.Domain.Entities;
using DesafioInvestimentosItau.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioInvestimentosItau.Infrastructure.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _context;

    public PositionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PositionEntity>> GetUserPositionsAsync(long userId)
    {
        return await _context.Positions
            .Include(p => p.Asset)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalProfitLossAsync(long userId)
    {
        var result = await _context.Positions
            .Where(p => p.UserId == userId)
            .Select(p => (decimal?)p.ProfitLoss)
            .SumAsync();

        return result ?? 0m;
    }
}