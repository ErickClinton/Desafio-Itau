using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;

public interface IQuoteRepository
{
    Task<bool> ExistsAsync(string assetCode, DateTime timestamp);
    Task<QuoteEntity> CreateAsync(QuoteEntity asset);
    Task<QuoteEntity?> GetLatestByAssetCodeAsync(string assetCode);
}