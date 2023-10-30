using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Input.Account;

public class ForgotPasswordInputModel
{
    [Required(ErrorMessage = "Электронная почта является обязательной")]
    [MaxLength(255)]
    [Display(Name = "Электронная почта")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public string? ReturnUrl { get; set; }
}