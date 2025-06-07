namespace DesafioInvestimentosItau.Application.User.User.Client;

public interface ITradeRepository
{
    Task<decimal> GetTotalInvestedAsync(long userId);
    Task<decimal> GetTotalBrokerageFeeAsync(long userId);
}