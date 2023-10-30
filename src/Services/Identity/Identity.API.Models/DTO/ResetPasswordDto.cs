namespace Identity.API.Models.DTO;

/// <summary>
/// DTO сброса пароля пользователя
/// </summary>
public record ResetPasswordDto(string UserName, string Token, string Password);
