using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract;

public interface IQuoteService
{
    Task<bool> ExistsAsync(string assetCode,DateTime timestamp);
    Task<QuoteEntity> CreateAsync(CreateQuoteDto assetDto);
}