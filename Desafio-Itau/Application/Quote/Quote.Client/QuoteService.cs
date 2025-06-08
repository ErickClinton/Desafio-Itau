using System.Text.Json;
using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Exceptions;
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
    private IAssetService _assetService;

    public QuoteService(IQuoteRepository quoteRepository, IQuoteInternalService quoteInternalService,
        IKafkaProducer kafkaProducer, ILogger<QuoteService> logger, IAssetService assetService)
    {
        _quoteRepository = quoteRepository;
        _quoteInternalService = quoteInternalService;
        _kafkaProducer = kafkaProducer;
        _logger = logger;
        _assetService = assetService;
    }

    public async Task<bool> ExistsAsync(string assetCode, DateTime timestamp)
    {
        return await _quoteRepository.ExistsAsync(assetCode, timestamp);
    }

    public async Task<QuotationMessageDto> SearchQuote(string assetCode)
    {
        _logger.LogInformation("Start Service SearchQuote - Request - {AssetCode}", assetCode);

        try
        {
            var quote = await _quoteInternalService.GetQuotationByAssetCodeAsync(assetCode);

            var messageDto = new QuotationMessageDto
            {
                AssetCode = quote.ticker,
                UnitPrice = quote.price,
                Timestamp = quote.tradeTime
            };

            var jsonMessage = JsonSerializer.Serialize(messageDto);

            _ = PublishToAllTopics(jsonMessage).ContinueWith(task =>
            {
                if (task.IsFaulted)
                    _logger.LogError(task.Exception, "Error publishing to topics");
            });

            _logger.LogInformation("End Service SearchQuote - Response - {@MessageDto}", messageDto);
            return messageDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch from B3 API. Trying fallback from database...");

            var latestQuote = await _quoteRepository.GetLatestByAssetCodeAsync(assetCode);
            if (latestQuote != null)
            {
                _logger.LogWarning("Returning latest quote from database for asset {AssetCode}", assetCode);

                var fallbackQuote = new QuotationMessageDto
                {
                    AssetCode = latestQuote.AssetCode,
                    UnitPrice = latestQuote.UnitPrice,
                    Timestamp = latestQuote.Timestamp
                };

                _logger.LogInformation("End Service SearchQuote - Response (fallback) - {@FallbackQuote}", fallbackQuote);
                return fallbackQuote;
            }

            _logger.LogError("No fallback quote available in database for asset {AssetCode}", assetCode);
            throw new FallBackException($"Unable to fetch quote for asset code '{assetCode}' from API or fallback.");
        }
    }
    
    private async Task PublishToAllTopics(string jsonMessage)
    {
        var topics = new[] { "quotation-topic", "asset-topic" };

        foreach (var topic in topics)
        {
            _logger.LogInformation("Try publish to {Topic}", topic);
            try
            {
                await _kafkaProducer.PublishAsync(topic, jsonMessage);
                _logger.LogInformation("Published to {Topic}", topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish to {Topic}", topic);
            }
        }
    }


public async Task<QuoteEntity> CreateAsync(CreateQuoteDto createQuoteDto)
    {
        _logger.LogInformation($"Start Service CreateAsync - Request - {createQuoteDto}");
        var quoteEntity = new QuoteEntity()
        {
            AssetCode = createQuoteDto.AssetCode , 
            UnitPrice = createQuoteDto.UnitPrice,
            Timestamp = createQuoteDto.Timestamp
        };
        
        var response = await _quoteRepository.CreateAsync(quoteEntity);
        _logger.LogInformation($"Start Service CreateAsync - Response - {response}");
        return response;
    }
}