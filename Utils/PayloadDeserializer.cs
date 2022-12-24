using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace SimpleEventDrivenKafka.Utils;

public class PayloadDeserializer<T> : IAsyncDeserializer<T> where T : class
{
    public Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
    {
        string json = Encoding.ASCII.GetString(data.Span);
        return Task.FromResult(JsonSerializer.Deserialize<T>(json))!;
    }
}