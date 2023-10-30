using Identity.API.Models.DTO;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Identity.API.MediatR.Commands.Register;

/// <summary>
/// Команда регистрации пользователя в системе
/// </summary>
public record RegisterCommand(RegisterDto Dto) : IRequest<IdentityResult>
{
}