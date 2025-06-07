using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;

public interface IAssetRepository
{ 
    Task<AssetEntity?> FindByAssetCodeAsync(string assetCode);
    Task<AssetEntity> CreateAsync(AssetEntity asset);
}