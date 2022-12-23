using Confluent.Kafka;

namespace SimpleEventDrivenKafka.Producers;

public class UserCreatedProducer
{
    private readonly ProducerConfig _producerConfig = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    
    public Object EmitMessage(string topic, string message)
    {
        using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
        {
            try
            {
                return producer.ProduceAsync(topic, new Message<Null, string> {Value = message})
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error: {error}");
            }
        }

        return null;
    }
}