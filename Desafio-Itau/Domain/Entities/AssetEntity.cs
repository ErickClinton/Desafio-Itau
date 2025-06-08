namespace DesafioInvestimentosItau.Domain.Entities;

public class AssetEntity
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}