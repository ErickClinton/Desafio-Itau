
namespace DesafioInvestimentosItau.Application.Investment.Investment.Contract.DTOs;

public class TopPositionsResponseDto : TopInvestmentsDto
{
    public int TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }
}