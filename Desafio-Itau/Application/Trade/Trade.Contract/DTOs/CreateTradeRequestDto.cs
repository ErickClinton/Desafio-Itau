namespace DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;

public class CreateTradeRequestDto
{
    public long UserId { get; set; }
    public string AssetCode { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}