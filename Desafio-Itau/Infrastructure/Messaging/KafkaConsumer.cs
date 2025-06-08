using Confluent.Kafka;
using DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Infrastructure.Messaging;

public class KafkaConsumer : IKafkaConsumer, IDisposable
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<KafkaConsumer> _logger;

    public KafkaConsumer(IConsumer<Ignore, string> consumer, ILogger<KafkaConsumer> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    public void Subscribe(string topic)
    {
        _logger.LogInformation("Start Subscribe to topic {Topic}", topic);
        _consumer.Subscribe(topic);
        _logger.LogInformation("Subscribed to topic {Topic}", topic);
    }

    public Task<string?> ConsumeAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            try
            {
                _logger.LogInformation("Waiting for Kafka message...");
                var result = _consumer.Consume(cancellationToken);
                _logger.LogInformation("Kafka message received from topic {Topic}, partition {Partition}, offset {Offset}",
                    result.Topic, result.Partition, result.Offset);
                return result?.Message?.Value;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }, cancellationToken);
    }

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }
}