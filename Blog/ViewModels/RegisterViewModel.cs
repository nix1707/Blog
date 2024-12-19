using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class RegisterViewModel
{
    [DataType(DataType.EmailAddress), Required]
    public string Email { get; set; }
    
    [DataType(DataType.Password), Required]
    public string Password { get; set; }

    [DataType(DataType.Password), Compare("Password"), Required]
    public string ConfirmPassword { get; set; }

}
