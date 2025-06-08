using DesafioInvestimentosItau.Application.Asset.Asset.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;

public interface IAssetService
{
    Task<AssetEntity> CreateAsync(CreateAssetRequest request);
    Task<bool> ExistsByCodeAsync(string code);
}