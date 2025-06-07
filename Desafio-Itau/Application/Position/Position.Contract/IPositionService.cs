using DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;
using DesafioInvestimentosItau.Application.User.User.Client.DTOs;

namespace DesafioInvestimentosItau.Application.Position.Position.Contract;

public interface IPositionService
{
    Task<List<AssetPositionDto>> GetUserPositionsAsync(long userId);
    Task<GlobalPositionDto> GetGlobalPositionAsync(long userId);
}