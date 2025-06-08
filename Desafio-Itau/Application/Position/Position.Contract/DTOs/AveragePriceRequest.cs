namespace DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;

public class AveragePriceRequest
{
    public long UserId { get; set; }
    public string AssetCode { get; set; }   
}