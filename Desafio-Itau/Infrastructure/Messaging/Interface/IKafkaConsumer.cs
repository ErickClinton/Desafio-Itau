namespace DesafioInvestimentosItau.Infrastructure.Messaging.Interface;

public interface IKafkaConsumer
{
    void Subscribe(string topic);
    Task<string> ConsumeAsync(CancellationToken cancellationToken);
}