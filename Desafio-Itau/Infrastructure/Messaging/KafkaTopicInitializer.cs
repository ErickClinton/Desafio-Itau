using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace DesafioInvestimentosItau.Infrastructure.Messaging;

public static class KafkaTopicInitializer
{
    public static async Task EnsureTopicsExistAsync(IEnumerable<string> topics, string bootstrapServers)
    {
        var config = new AdminClientConfig() { BootstrapServers = bootstrapServers };

        using var adminClient = new AdminClientBuilder(config).Build();

        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
        var existingTopics = metadata.Topics.Select(t => t.Topic).ToHashSet();

        var topicsToCreate = topics
            .Where(t => !existingTopics.Contains(t))
            .Select(t => new TopicSpecification()
            {
                Name = t,
                NumPartitions = 1,
                ReplicationFactor = 1
            })
            .ToList();

        if (topicsToCreate.Count == 0)
            return;

        try
        {
            await adminClient.CreateTopicsAsync(topicsToCreate);
        }
        catch (CreateTopicsException ex)
        {
            foreach (var result in ex.Results)
            {
                if (result.Error.Code != ErrorCode.TopicAlreadyExists)
                {
                    throw; 
                }
            }
        }
    }
}