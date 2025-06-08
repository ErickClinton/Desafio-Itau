using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
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
    
    public async Task<List<TopPositionDto>> GetTopUserPositionsAsync(int top)
    {
        var result = await _context.Positions
            .GroupBy(p => new { p.UserId, p.User.Name, p.User.Email })
            .Select(g => new TopPositionDto
            {
                UserId = g.Key.UserId,
                UserName = g.Key.Name,
                Email = g.Key.Email,
                TotalQuantity = g.Sum(p => p.Quantity),
                TotalValue = g.Sum(p => p.Quantity * p.AveragePrice)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(top)
            .ToListAsync();

        return result;
    }
    
    public async Task<PositionEntity?> GetAveragePriceAsync(long userId, string assetCode)
    {
        return await _context.Positions
            .Where(p => p.UserId == userId && p.AssetCode == assetCode)
            .FirstOrDefaultAsync();
    }
}