using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;

public interface IAssetRepository
{
    Task<AssetEntity> CreateAsync(AssetEntity asset);
    Task<bool> ExistsByCodeAsync(string code);
}