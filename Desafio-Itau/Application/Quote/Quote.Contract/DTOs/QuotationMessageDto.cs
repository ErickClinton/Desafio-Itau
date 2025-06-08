namespace DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;

public class QuotationMessageDto
{
    public string AssetCode { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public DateTime Timestamp { get; set; }
}