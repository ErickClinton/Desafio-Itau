using DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;

namespace DesafioInvestimentosItau.Application.Investment.Investment.Contract.DTOs;

public class TopInvestmentsResponseDto
{
    public List<TopPositionsResponseDto> TopByQuantity { get; set; } = new();
    public List<TopBrokerageFeeResponseDto> TopByBrokerage { get; set; } = new();
}