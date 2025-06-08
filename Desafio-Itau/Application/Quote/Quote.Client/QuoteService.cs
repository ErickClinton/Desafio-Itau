using System.Text.Json;
using DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Application.Quote.Quote.Client;

public class QuoteService : IQuoteService
{
    private IQuoteRepository _quoteRepository;
    private IQuoteInternalService _quoteInternalService;
    private IKafkaProducer _kafkaProducer;
    private ILogger<QuoteService> _logger;

    public QuoteService(IQuoteRepository quoteRepository, IQuoteInternalService quoteInternalService,
        IKafkaProducer kafkaProducer, ILogger<QuoteService> logger)
    {
        _quoteRepository = quoteRepository;
        _quoteInternalService = quoteInternalService;
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    public async Task<bool> ExistsAsync(string assetCode, DateTime timestamp)
    {
        return await _quoteRepository.ExistsAsync(assetCode, timestamp);
    }

    public async Task<QuotationMessageDto> SearchQuote(string assetCode)
    {
        try
        {
            var quote = await _quoteInternalService.GetQuotationByAssetCodeAsync(assetCode);
            var messageDto = new QuotationMessageDto()
            {
                AssetCode = quote.ticker,
                UnitPrice = quote.price,
                Timestamp = quote.tradeTime
            };
            var jsonMessage = JsonSerializer.Serialize(messageDto);

            await _kafkaProducer.PublishAsync("quotation-topic", jsonMessage);
            return messageDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch from B3 API. Trying fallback from database...");

            var latestQuote = await _quoteRepository.GetLatestByAssetCodeAsync(assetCode);
            if (latestQuote != null)
            {
                _logger.LogWarning("Returning latest quote from database for asset {AssetCode}", assetCode);
                var quote = new QuotationMessageDto()
                {
                    AssetCode = latestQuote.Asset.Code,
                    UnitPrice = latestQuote.UnitPrice,
                    Timestamp = latestQuote.Timestamp
                };
                return quote;
            }

            _logger.LogError("No fallback quote available in database for asset {AssetCode}", assetCode);
            throw new Exception($"Unable to fetch quote for asset code {assetCode} from API or fallback.");
        }
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