using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Input.Account;

public class LoginInputModel
{
    [Required(ErrorMessage = "��� ������������ �� �������")]
    [MaxLength(255)]
    [Display(Name = "��� ������������")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "������ �������� ������������")]
    [MaxLength(255)]
    [Display(Name = "������")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "���������?")]
    public bool RememberLogin { get; set; }
    public string? ReturnUrl { get; set; }
}
