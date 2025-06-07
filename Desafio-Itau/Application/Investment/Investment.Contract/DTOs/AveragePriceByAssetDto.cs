namespace DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;

public class AveragePriceByAssetDto
{
    public string AssetCode { get; set; } = null!;  
    public decimal AveragePrice { get; set; }  
}