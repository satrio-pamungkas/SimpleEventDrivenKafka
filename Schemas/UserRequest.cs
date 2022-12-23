using System.ComponentModel.DataAnnotations;

namespace SimpleEventDrivenKafka.Schemas;

public class UserRequest
{
    [Required]
    public String? FirstName { get; set; }
    
    [Required]
    public String? LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public String? Email { get; set; }
    
    [Required]
    public String? PhoneNumber { get; set; }
}