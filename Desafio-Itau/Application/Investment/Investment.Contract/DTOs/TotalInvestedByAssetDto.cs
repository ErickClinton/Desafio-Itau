namespace DesafioInvestimentosItau.Application.Investment.Investment.Contract.DTOs;

public class TotalInvestedByAssetDto
{
    public string AssetCode { get; set; } = null!;
    public decimal TotalInvest { get; set; }
}