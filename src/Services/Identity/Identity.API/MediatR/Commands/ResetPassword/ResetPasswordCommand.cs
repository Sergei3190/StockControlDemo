using Identity.API.Models.DTO;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Identity.API.MediatR.Commands.ResetPassword;

/// <summary>
/// Команда сброса пароля пользователя
/// </summary>
public record ResetPasswordCommand(ResetPasswordDto Dto) : IRequest<IdentityResult>
{
}