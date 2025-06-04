namespace DesafioInvestimentosItau.Domain.Entities;

public class QuoteEntity
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime Timestamp { get; set; }

    public AssetEntity? Asset { get; set; }
}