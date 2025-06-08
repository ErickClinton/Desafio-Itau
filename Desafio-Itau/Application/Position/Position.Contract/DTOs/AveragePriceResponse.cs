namespace DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;

public class AveragePriceResponse
{
    
    public string AssetCode { get; set; } = null!;  
    public decimal AveragePrice { get; set; }  
    
}