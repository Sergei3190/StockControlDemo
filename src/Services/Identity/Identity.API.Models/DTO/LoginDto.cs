using Identity.API.Models.Input.Account;

namespace Identity.API.Models.DTO;

/// <summary>
/// DTO входа пользователя в ситсему
/// </summary>
public record LoginDto(LoginInputModel Model);