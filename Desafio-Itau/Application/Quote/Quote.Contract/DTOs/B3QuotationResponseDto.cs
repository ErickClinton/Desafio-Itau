namespace DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;

public class B3QuotationResponseDto
{
    public decimal price { get; set; }
    public decimal priceOpen { get; set; }
    public decimal high { get; set; }
    public decimal low { get; set; }
    public long volume { get; set; }
    public long marketCap { get; set; }
    public DateTime tradeTime { get; set; }
    public long volumeAvg { get; set; }
    public decimal pe { get; set; }
    public decimal eps { get; set; }
    public decimal high52 { get; set; }
    public decimal low52 { get; set; }
    public decimal change { get; set; }
    public decimal changePct { get; set; }
    public decimal closeYest { get; set; }
    public long shares { get; set; }
    public string ticker { get; set; } = string.Empty;
}