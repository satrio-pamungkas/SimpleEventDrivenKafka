namespace SimpleEventDrivenKafka.Handlers;

public class SmsHandler
{
    public void SendSms(string phoneNumber)
    {
        Console.WriteLine($"SMS Being sent to {phoneNumber}");
    }
}