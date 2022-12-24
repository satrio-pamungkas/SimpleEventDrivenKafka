using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace SimpleEventDrivenKafka.Utils;

public class PayloadSerializer<T> : IAsyncSerializer<T> where T : class
{
    Task<byte[]> IAsyncSerializer<T>.SerializeAsync(T data, SerializationContext context)
    {
        string jsonString = JsonSerializer.Serialize(data);
        return Task.FromResult(Encoding.ASCII.GetBytes(jsonString));
    }
}