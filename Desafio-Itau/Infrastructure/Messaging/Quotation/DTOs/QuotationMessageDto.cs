namespace DesafioInvestimentosItau.Infrastructure.Messaging.Quotation.DTOs;

public class QuotationMessageDto
{
    public string AssetCode { get; set; } = null!;
    public string AssetName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public DateTime Timestamp { get; set; }
}