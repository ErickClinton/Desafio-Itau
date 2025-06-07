using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;

namespace DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Client;

public class QuoteService: IQuoteService
{
    private IQuoteRepository _quoteRepository;

    public QuoteService(IQuoteRepository quoteRepository)
    {
        _quoteRepository = quoteRepository;
    }
    
    public async Task<bool> ExistsAsync(string assetCode, DateTime timestamp)
    {
        return await _quoteRepository.ExistsAsync(assetCode,timestamp);
    }

    public async Task<QuoteEntity> CreateAsync(CreateQuoteDto createQuoteDto)
    {
        var quoteEntity = new QuoteEntity()
        {
            AssetId =createQuoteDto.AssetId , 
            UnitPrice = createQuoteDto.UnitPrice,
            Timestamp = createQuoteDto.Timestamp
        };
        return await _quoteRepository.CreateAsync(quoteEntity);
    }
}