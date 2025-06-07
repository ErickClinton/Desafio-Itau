using DesafioInvestimentosItau.Application.User.User.Client.DTOs;

namespace DesafioInvestimentosItau.Application.User.User.Client;

public interface IPositionRepository
{
    Task<IEnumerable<AssetPositionDto>> GetUserPositionsAsync(long userId);
    Task<decimal> GetTotalProfitLossAsync(long userId);
}