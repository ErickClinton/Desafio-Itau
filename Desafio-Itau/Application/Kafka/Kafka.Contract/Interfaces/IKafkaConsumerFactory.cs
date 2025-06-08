namespace DesafioInvestimentosItau.Application.Kafka.Kafka.Contract.Interfaces;

public interface IKafkaConsumerFactory
{
    IKafkaConsumer Create(string groupId);
}