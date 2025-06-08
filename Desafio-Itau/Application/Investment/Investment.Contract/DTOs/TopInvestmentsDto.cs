namespace DesafioInvestimentosItau.Application.Investment.Investment.Contract.DTOs;

public class TopInvestmentsDto
{
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
