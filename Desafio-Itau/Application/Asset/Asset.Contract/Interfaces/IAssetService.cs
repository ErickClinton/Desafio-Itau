using DesafioInvestimentosItau.Application.User.User.Client.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;

public interface IAssetService
{
    Task<AssetEntity?> GetByAssetCode(string assetCode);
    Task<AssetEntity> CreateAsync(CreateAssetDto assetDto);
}