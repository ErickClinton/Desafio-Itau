using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Dtos;
using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Asset.Asset.Client;

public class AssetService: IAssetService
{
    private IAssetRepository _assetRepository;
    
    public AssetService(IAssetRepository tradeRepository)
    {
        _assetRepository = tradeRepository;
    }

    public async Task<AssetEntity?> GetByAssetCode(string assetCode)
    {
        return await _assetRepository.FindByAssetCodeAsync(assetCode);
    }

    public async Task<AssetEntity> CreateAsync(CreateAssetDto createAssetDto)
    {
        var assetEntity = new AssetEntity() { Code = createAssetDto.Code };
        return await _assetRepository.CreateAsync(assetEntity);
    }
}