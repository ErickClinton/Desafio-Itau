using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;
using DesafioInvestimentosItau.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioInvestimentosItau.Infrastructure.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly ApplicationDbContext _context;

    public AssetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssetEntity> CreateAsync(AssetEntity asset)
    {
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return asset;
    }
    
    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Assets.AnyAsync(a => a.Code == code);
    }
}