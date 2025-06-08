namespace DesafioInvestimentosItau.Application.Position.Position.Contract.DTOs;

public class TopPositionDto
{
    public long UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public int TotalQuantity { get; set; }
    public decimal TotalValue { get; set; }

}