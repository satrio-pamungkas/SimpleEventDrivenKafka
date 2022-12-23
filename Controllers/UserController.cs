using Microsoft.AspNetCore.Mvc;
using SimpleEventDrivenKafka.Models;
using SimpleEventDrivenKafka.Schemas;

namespace SimpleEventDrivenKafka.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
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

        return data;
    }
}