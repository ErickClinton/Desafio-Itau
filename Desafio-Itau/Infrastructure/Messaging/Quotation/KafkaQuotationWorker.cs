using System.Text.Json;
using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;
using DesafioInvestimentosItau.Application.User.User.Client.DTOs;
using DesafioInvestimentosItau.Infrastructure.Data;
using DesafioInvestimentosItau.Infrastructure.Messaging.Interface;
using DesafioInvestimentosItau.Infrastructure.Messaging.Quotation.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Infrastructure.Messaging.Quotation;

public class KafkaQuotationWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IKafkaConsumer _kafkaConsumer;
    private readonly ILogger<KafkaQuotationWorker> _logger;

    public KafkaQuotationWorker(
        IServiceScopeFactory scopeFactory, 
        IKafkaConsumer kafkaConsumer, 
        ILogger<KafkaQuotationWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _kafkaConsumer = kafkaConsumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _kafkaConsumer.Subscribe("quotation-topic");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var rawMessage = await _kafkaConsumer.ConsumeAsync(stoppingToken);
                var message = JsonSerializer.Deserialize<QuotationMessageDto>(rawMessage, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (message != null)
                    await ProcessMessageAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message from Kafka.");
                await Task.Delay(1000);
            }
        }
    }

    private async Task ProcessMessageAsync(QuotationMessageDto message)
{
    using var scope = _scopeFactory.CreateScope();
    var assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
    var quoteService = scope.ServiceProvider.GetRequiredService<IQuoteService>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    using var transaction = await dbContext.Database.BeginTransactionAsync();
    
    try
    {
        _logger.LogInformation("Processing new quotation message for AssetCode: {AssetCode}, Timestamp: {Timestamp}", message.AssetCode, message.Timestamp);

        var asset = await assetService.GetByAssetCode(message.AssetCode);
        if (asset == null)
        {
            _logger.LogInformation("Asset not found. Creating new asset with Code: {AssetCode}", message.AssetCode);

            var createAsset = new CreateAssetDto
            {
                Name = message.AssetName,
                Code = message.AssetCode,
            };
            asset = await assetService.CreateAsync(createAsset);

            _logger.LogInformation("Asset created with ID: {AssetId}", asset.Id);
        }
        else
        {
            _logger.LogInformation("Asset already exists with ID: {AssetId}", asset.Id);
        }

        var exists = await quoteService.ExistsAsync(message.AssetCode, message.Timestamp);
        if (!exists)
        {
            _logger.LogInformation("Quotation not found. Creating new quotation for Asset ID: {AssetId}", asset.Id);

            var createQuotation = new CreateQuoteDto
            {
                AssetId = asset.Id,
                Timestamp = message.Timestamp,
                UnitPrice = message.UnitPrice,
            };

            await quoteService.CreateAsync(createQuotation);

            _logger.LogInformation("Quotation successfully created.");
        }
        else
        {
            _logger.LogInformation("Quotation already exists for AssetCode: {AssetCode} at {Timestamp}", message.AssetCode, message.Timestamp);
        }

        await transaction.CommitAsync();
        _logger.LogInformation("Transaction committed successfully.");
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        _logger.LogError(ex, "Error processing quotation message. Transaction rolled back.");
        throw;
    }
}
}
