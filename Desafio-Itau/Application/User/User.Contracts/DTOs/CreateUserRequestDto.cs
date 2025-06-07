namespace DesafioInvestimentosItau.Application.User.User.Client.DTOs;

public class CreateUserRequestDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public decimal BrokerageFee { get; set; }
}