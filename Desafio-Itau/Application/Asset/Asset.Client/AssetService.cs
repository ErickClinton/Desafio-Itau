using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;
using DesafioInvestimentosItau.Application.User.User.Client;
using DesafioInvestimentosItau.Application.User.User.Client.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Asset.Asset.Client;

public class AssetService
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
        var assetEntity = new AssetEntity() { Code = createAssetDto.Code, Name = createAssetDto.Name };
        return await _assetRepository.CreateAsync(assetEntity);
    }
}