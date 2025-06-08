namespace DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;

public interface IKafkaConsumer
{
    void Subscribe(string topic);
    Task<string> ConsumeAsync(CancellationToken cancellationToken);
}