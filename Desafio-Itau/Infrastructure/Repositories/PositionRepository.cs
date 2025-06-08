using DesafioInvestimentosItau.Application.Position.Position.Contract.Interfaces;
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
    
    public async Task<PositionEntity> CreateAsync(PositionEntity position)
    {
        _context.Positions.Add(position);
        await _context.SaveChangesAsync();
        return position;
    }
    
    public async Task<PositionEntity?> GetByUserAndAssetAsync(long userId, string assetCode)
    {
        return await _context.Positions
            .FirstOrDefaultAsync(p => p.UserId == userId && p.AssetCode == assetCode);
    }
    
    public async Task UpdateAsync(PositionEntity position)
    {
        _context.Positions.Update(position);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PositionEntity>> GetUserPositionsAsync(long userId)
    {
        return await _context.Positions
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
    
    public async Task<PositionEntity?> GetAveragePriceAsync(long userId, string assetCode)
    {
        return await _context.Positions
            .Where(p => p.UserId == userId && p.AssetCode == assetCode)
            .FirstOrDefaultAsync();
    }
}