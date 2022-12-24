namespace SimpleEventDrivenKafka.Handlers;

public class EmailHandler
{
    public void SendEmail(string email)
    {
        Console.WriteLine($"Email being sent to {email}");
    }
}