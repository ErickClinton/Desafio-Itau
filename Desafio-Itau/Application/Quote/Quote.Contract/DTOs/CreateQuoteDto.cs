namespace DesafioInvestimentosItau.Application.Quote.Quote.Contract.DTOs;

public class CreateQuoteDto
{
    public long AssetId { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime Timestamp { get; set; }
}