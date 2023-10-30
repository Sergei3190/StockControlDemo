using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Input.Account;

public class RegisterInputModel : LoginInputModel
{
    [Required(ErrorMessage = "Электронная почта является обязательной")]
    [MaxLength(255)]
    [Display(Name = "Электронная почта")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Не введено подтверждение пароля")]
    [MaxLength(255)]
    [Display(Name = "Подтверждение пароля")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Пароль и подтверждение не совпадают")]
    public string PasswordConfirm { get; set; }
}
