namespace DesafioInvestimentosItau.Application.Trade.Trade.Contract.DTOs;

public class TopBrokerageDto
{
    public long UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public decimal TotalBrokerage { get; set; }
}