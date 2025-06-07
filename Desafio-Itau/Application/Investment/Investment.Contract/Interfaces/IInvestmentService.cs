using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;
using DesafioInvestimentosItau.Application.User.User.Client.DTOs;

namespace DesafioInvestimentosItau.Application.Investment.Investment.Contract.Interfaces;

public interface IInvestmentService
{
    Task<List<TotalInvestedByAssetDto>> GetAllInvestmentsByUserIdAsync(long userId);
    Task<List<AveragePriceByAssetDto>> GetAveragePricePerAssetAsync(long userId);
    Task<List<AssetPositionDto>> GetUserPositionsAsync(long userId);
}