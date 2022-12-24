using Microsoft.AspNetCore.Mvc;
using SimpleEventDrivenKafka.Models;
using SimpleEventDrivenKafka.Schemas;
using SimpleEventDrivenKafka.Producers;

namespace SimpleEventDrivenKafka.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserCreatedProducer _userCreatedProducer;
    private readonly string _topic;

    public UserController(IConfiguration config)
    {
        this._userCreatedProducer = new UserCreatedProducer();
        this._topic = config.GetValue<string>("Kafka:Topic");
    }
    
    [HttpPost]
    public ActionResult<User> Register (UserRequest request)
    {
        var data = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };
        this._userCreatedProducer.EmitMessage(this._topic, data);
        
        return data;
    }
}