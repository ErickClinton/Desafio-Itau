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
            _logger.LogInformation("Enviando mensagem para Kafka no tópico {Topic}: {Message}", topic, message);
        
            var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        
            _producer.Flush(TimeSpan.FromSeconds(5));
            _logger.LogInformation("Mensagem publicada com sucesso no Kafka: {Offset}", result.Offset);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar no Kafka: {Message}", ex.Message);
            throw;
        }
    }
}