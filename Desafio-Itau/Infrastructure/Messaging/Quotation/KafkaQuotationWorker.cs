using System.Text.Json;
using DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Infrastructure.Messaging.Quotation;

public class KafkaQuotationWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KafkaQuotationWorker> _logger;
    private readonly IKafkaConsumer _kafkaConsumer;
    public KafkaQuotationWorker(
        IServiceScopeFactory scopeFactory,
        IKafkaConsumerFactory kafkaConsumerFactory,
        ILogger<KafkaQuotationWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _kafkaConsumer = kafkaConsumerFactory.Create("quotation-consumer");
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _kafkaConsumer.Subscribe("quotation-topic");
        _logger.LogInformation($"Start Kafka - Request - {stoppingToken}");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var rawMessage = await _kafkaConsumer.ConsumeAsync(stoppingToken);
                
                if (string.IsNullOrWhiteSpace(rawMessage)) continue;

                var message = JsonSerializer.Deserialize<QuotationMessageDto>(rawMessage,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (message != null)
                    await ProcessMessageAsync(message);
                _logger.LogInformation($"End Kafka");
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Kafka consumption cancelled.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message from Kafka.");
                await Task.Delay(1000, stoppingToken);
            }
        }

        _logger.LogInformation("KafkaQuotationWorker stopped.");
    }

    private async Task ProcessMessageAsync(QuotationMessageDto message)
    {
        _logger.LogInformation($"Start Kafka ProcessMessageAsync- Request - {message}");
        using var scope = _scopeFactory.CreateScope();
        var quoteService = scope.ServiceProvider.GetRequiredService<IQuoteService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            _logger.LogInformation("Processing new quotation message for AssetCode: {AssetCode}, Timestamp: {Timestamp}", message.AssetCode, message.Timestamp);
            
            var exists = await quoteService.ExistsAsync(message.AssetCode, message.Timestamp);
            if (!exists)
            {
                _logger.LogInformation("Quotation not found. Creating for Asset ID: {AssetId}", message.AssetCode);

                var createQuotation = new CreateQuoteDto()
                {
                    AssetCode = message.AssetCode,
                    Timestamp = message.Timestamp,
                    UnitPrice = message.UnitPrice
                };

                await quoteService.CreateAsync(createQuotation);
                _logger.LogInformation("Quotation created successfully.");
            }
            else
            {
                _logger.LogInformation("Quotation already exists for AssetCode: {AssetCode} at {Timestamp}", message.AssetCode, message.Timestamp);
            }

            await transaction.CommitAsync();
            _logger.LogInformation("Transaction committed.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error processing quotation. Transaction rolled back.");
            throw new Exception("Error in consumer kafka");
        }
    }
}
