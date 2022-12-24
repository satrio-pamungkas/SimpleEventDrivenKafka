using System.Text.Json;
using Confluent.Kafka;

namespace SimpleEventDrivenKafka.Subscribers;

public class UserCreatedSubscriber : BackgroundService
{
    private readonly string _topic;
    private readonly IConsumer<Ignore, string> _kafkaConsumer;

    public UserCreatedSubscriber(IConfiguration config)
    {
        var consumerConfig = new ConsumerConfig();
        config.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
        this._topic = config.GetValue<string>("Kafka:Topic");
        this._kafkaConsumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
    }
    
    private void StartConsumerLoop(CancellationToken cancellationToken)
    {
        this._kafkaConsumer.Subscribe(this._topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumer = this._kafkaConsumer.Consume(cancellationToken);

                // Handle message...
                // var message = JsonSerializer.Deserialize<Message<Null, string>>(cr.Message.Value);
                Console.WriteLine($"Message : {consumer.Message.Value}");
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException e)
            {
                // Consumer errors should generally be ignored (or logged) unless fatal.
                Console.WriteLine($"Consume error: {e.Error.Reason}");

                if (e.Error.IsFatal)
                {
                    // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e}");
                break;
            }
        }
    }
    
    public override void Dispose()
    {
        this._kafkaConsumer.Close(); // Commit offsets and leave the group cleanly.
        this._kafkaConsumer.Dispose();

        base.Dispose();
    }
}