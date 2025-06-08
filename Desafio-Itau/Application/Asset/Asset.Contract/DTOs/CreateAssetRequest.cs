namespace DesafioInvestimentosItau.Application.Asset.Asset.Contract.DTOs;

public class CreateAssetRequest
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}