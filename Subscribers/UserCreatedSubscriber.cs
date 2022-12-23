using Confluent.Kafka;

namespace SimpleEventDrivenKafka.Subscribers;

public class UserCreatedSubscriber : IHostedService
{
    private readonly string topic = "registration";
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = "user_consumer_group",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var builder = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            builder.Subscribe(topic);
            var cancelToken = new CancellationTokenSource();
            try
            {
                while (true)
                {
                    var consumer = builder.Consume(cancelToken.Token);
                    Console.WriteLine($"First Name is : {consumer.Message.Value}");
                }
            }
            catch (Exception)
            {
                builder.Close();
            }
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}