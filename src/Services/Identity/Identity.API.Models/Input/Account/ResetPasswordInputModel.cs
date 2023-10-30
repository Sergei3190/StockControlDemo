using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Input.Account;

public class ResetPasswordInputModel
{
    [Required(ErrorMessage = "Пароль является обязательным")]
    [MaxLength(255)]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Не введено подтверждение пароля")]
    [MaxLength(255)]
    [Display(Name = "Подтверждение пароля")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Пароль и подтверждение не совпадают")]
    public string PasswordConfirm { get; set; }

    public string Email { get; set; }
    public string Token { get; set; }

    /// <summary>
    /// Клиент, с которого был направлен запрос авторизации
    /// </summary>
    public string Client { get; set; }
}
