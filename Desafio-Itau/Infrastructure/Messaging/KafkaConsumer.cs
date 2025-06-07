using Confluent.Kafka;
using DesafioInvestimentosItau.Infrastructure.Messaging.Interface;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Infrastructure.Messaging;

public class KafkaConsumer : IKafkaConsumer, IDisposable
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<KafkaConsumer> _logger;

    public KafkaConsumer(ILogger<KafkaConsumer> logger)
    {
        _logger = logger;

        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "quotation-consumer",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    public void Subscribe(string topic)
    {
        _consumer.Subscribe(topic);
        _logger.LogInformation("Subscribed to topic {Topic}", topic);
    }

    public Task<string> ConsumeAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = _consumer.Consume(cancellationToken);
            return Task.FromResult(result.Message.Value);
        }
        catch (ConsumeException ex)
        {
            _logger.LogError(ex, "Kafka consume error");
            throw;
        }
    }

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }
}