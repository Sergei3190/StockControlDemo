using Identity.API.Models.DTO;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Identity.API.MediatR.Commands.Login;

/// <summary>
/// Команда входа пользователя в систему
/// </summary>
public record LoginCommand(LoginDto Dto) : IRequest<SignInResult>
{
}