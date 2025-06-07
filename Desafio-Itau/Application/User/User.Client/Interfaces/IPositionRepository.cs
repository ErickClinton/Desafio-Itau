using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.User.User.Client;

public interface IPositionRepository
{
    Task<IEnumerable<PositionEntity>> GetUserPositionsAsync(long userId);
    Task<decimal> GetTotalProfitLossAsync(long userId);
}