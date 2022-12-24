using Confluent.Kafka;

namespace SimpleEventDrivenKafka.Producers;

public class UserCreatedProducer
{
    private readonly ProducerConfig _producerConfig = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    
    public void EmitMessage(string topic, string message)
    {
        var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
    
        producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

    }
}