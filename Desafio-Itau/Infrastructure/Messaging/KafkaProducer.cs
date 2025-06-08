using Confluent.Kafka;
using DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DesafioInvestimentosItau.Infrastructure.Messaging;


public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    private ILogger<KafkaProducer> _logger;

    public KafkaProducer(IConfiguration configuration,ILogger<KafkaProducer> logger)
    {
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            MessageTimeoutMs = 3000,          
            RequestTimeoutMs = 5000,         
            SocketTimeoutMs = 3000,           
            Acks = Acks.Leader,               
            EnableIdempotence = false         
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync(string topic, string message)
    {
        try
        {
            _logger.LogInformation($"Start PublishAsync - Request - {topic} - {message}");        
            var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        
            _producer.Flush(TimeSpan.FromSeconds(5));
             _logger.LogInformation($"End PublishAsync - response - {topic} - {result.Offset}");      
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error send to kafka: {Message}", ex.Message);
            throw;
        }
    }
}