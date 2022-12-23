using Microsoft.AspNetCore.Mvc;
using SimpleEventDrivenKafka.Models;
using SimpleEventDrivenKafka.Schemas;
using SimpleEventDrivenKafka.Producers;

namespace SimpleEventDrivenKafka.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserCreatedProducer _userCreatedProducer = new UserCreatedProducer();
    private readonly string topic = "registration";
    [HttpPost]
    public ActionResult<User> Register (UserRequest request)
    {
        _userCreatedProducer.EmitMessage(topic, request.FirstName!);
        var data = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };
        
        return data;
    }
}