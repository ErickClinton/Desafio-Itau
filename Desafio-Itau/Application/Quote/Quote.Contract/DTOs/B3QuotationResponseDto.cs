using System.Text.Json.Serialization;

namespace DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;

public class B3QuotationResponseDto
{
    public decimal price { get; set; }

    [JsonPropertyName("priceopen")]
    public decimal priceOpen { get; set; }

    public decimal high { get; set; }
    public decimal low { get; set; }
    public long volume { get; set; }

    [JsonPropertyName("marketcap")]
    public long marketCap { get; set; }

    [JsonPropertyName("tradetime")]
    public DateTime tradeTime { get; set; }

    [JsonPropertyName("volumeavg")]
    public long volumeAvg { get; set; }

    public decimal? pe { get; set; }
    public decimal? eps { get; set; }

    public decimal high52 { get; set; }
    public decimal low52 { get; set; }
    public decimal change { get; set; }

    [JsonPropertyName("changepct")]
    public decimal changePct { get; set; }

    [JsonPropertyName("closeyest")]
    public decimal closeYest { get; set; }

    public long shares { get; set; }
    public string ticker { get; set; } = string.Empty;
}