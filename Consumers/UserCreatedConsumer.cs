// using System.Text.Json;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using SimpleEventDrivenKafka.Handlers;
using SimpleEventDrivenKafka.Models;
using SimpleEventDrivenKafka.Utils;

namespace SimpleEventDrivenKafka.Consumers;

public class UserCreatedConsumer : BackgroundService
{
    private readonly string _topic;
    private readonly IConsumer<Ignore, User> _kafkaConsumer;
    private readonly EmailHandler _emailHandler;
    private readonly SmsHandler _smsHandler;

    public UserCreatedConsumer(IConfiguration config)
    {
        var consumerConfig = new ConsumerConfig();
        config.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
        this._topic = config.GetValue<string>("Kafka:Topic");
        this._kafkaConsumer = new ConsumerBuilder<Ignore, User>(consumerConfig)
            .SetValueDeserializer(new PayloadDeserializer<User>().AsSyncOverAsync())
            .Build();
        this._emailHandler = new EmailHandler();
        this._smsHandler = new SmsHandler();
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
                this._emailHandler.SendEmail(consumer.Message.Value.Email!);
                this._smsHandler.SendSms(consumer.Message.Value.PhoneNumber!);
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