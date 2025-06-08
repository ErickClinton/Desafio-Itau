using System.Text.Json;
using DesafioInvestimentosItau.Application.Asset.Asset.Contract.DTOs;
using DesafioInvestimentosItau.Application.Asset.Asset.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;
using DesafioInvestimentosItau.Application.Quote.Quote.Contract.Quote.Contract.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Infrastructure.Messaging.Asset;

public class KafkaAssetWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IKafkaConsumer _kafkaConsumer;
    private readonly ILogger _logger;

    public KafkaAssetWorker(
        IServiceScopeFactory scopeFactory,
        IKafkaConsumerFactory kafkaConsumerFactory,
        ILogger<KafkaAssetWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _kafkaConsumer = kafkaConsumerFactory.Create("asset-consumer");
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _kafkaConsumer.Subscribe("asset-topic");
        _logger.LogInformation("Start KafkaAssetWorker - Listening to 'asset-topic'");

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
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("KafkaAssetWorker cancelled.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming asset message from Kafka.");
                await Task.Delay(1000, stoppingToken);
            }
        }

        _logger.LogInformation("KafkaAssetWorker stopped.");
    }

    private async Task ProcessMessageAsync(QuotationMessageDto message)
    {
        _logger.LogInformation("Processing asset from quotation message - AssetCode: {AssetCode}", message.AssetCode);

        using var scope = _scopeFactory.CreateScope();
        var assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();

        var existing = await assetService.ExistsByCodeAsync(message.AssetCode);
        if (existing)
        {
            _logger.LogInformation("Asset already exists for code: {AssetCode}", message.AssetCode);
            return;
        }

        var newAsset = new CreateAssetRequest()
        {
            Code = message.AssetCode,
            Name = $"Asset {message.AssetCode}"
        };

        await assetService.CreateAsync(newAsset);
        _logger.LogInformation("Asset created successfully for code: {AssetCode}", message.AssetCode);
    }
}