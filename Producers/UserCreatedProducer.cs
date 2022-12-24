using Confluent.Kafka;
using SimpleEventDrivenKafka.Utils;
using SimpleEventDrivenKafka.Models;

namespace SimpleEventDrivenKafka.Producers;

public class UserCreatedProducer
{
    private readonly ProducerConfig _producerConfig = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    
    public void EmitMessage(string topic, User payload)
    {
        var producer = new ProducerBuilder<Null, User>(_producerConfig)
            .SetValueSerializer(new PayloadSerializer<User>())
            .Build();
    
        producer.ProduceAsync(topic, new Message<Null, User> { Value = payload });

    }
}