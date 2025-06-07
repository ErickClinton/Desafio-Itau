using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract;

public interface IQuoteRepository
{
    Task<bool> ExistsAsync(string assetCode, DateTime timestamp);
    Task<QuoteEntity> CreateAsync(QuoteEntity asset);
}