using Confluent.Kafka;
using DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Infrastructure.Messaging.Factory;

public class KafkaConsumerFactory : IKafkaConsumerFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public KafkaConsumerFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public IKafkaConsumer Create(string groupId)
    {
        var logger = _loggerFactory.CreateLogger<KafkaConsumer>();

        var config = new ConsumerConfig()
        {
            BootstrapServers = "localhost:9092",
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        return new KafkaConsumer(new ConsumerBuilder<Ignore, string>(config).Build(), logger);
    }
}
