namespace DesafioInvestimentosItau.Api.Models;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}