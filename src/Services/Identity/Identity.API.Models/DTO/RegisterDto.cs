using Identity.API.Models.Input.Account;

namespace Identity.API.Models.DTO;

/// <summary>
/// DTO регистрации пользователя
/// </summary>
public record RegisterDto(RegisterInputModel Model, SendEmailDto EmailDto);