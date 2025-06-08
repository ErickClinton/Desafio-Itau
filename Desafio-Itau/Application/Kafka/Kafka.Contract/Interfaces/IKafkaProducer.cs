namespace DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;

public interface IKafkaProducer
{
    Task PublishAsync(string topic, string message);
}