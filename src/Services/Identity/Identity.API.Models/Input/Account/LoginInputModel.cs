using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Input.Account;

public class LoginInputModel
{
    [Required(ErrorMessage = "Имя пользователя не указано")]
    [MaxLength(255)]
    [Display(Name = "Имя пользователя")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Пароль является обязательным")]
    [MaxLength(255)]
    [Display(Name = "Пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Запомнить?")]
    public bool RememberLogin { get; set; }
    public string? ReturnUrl { get; set; }
}
