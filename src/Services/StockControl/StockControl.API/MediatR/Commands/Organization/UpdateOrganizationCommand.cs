using MediatR;

using StockControl.API.Models.DTO.Organization;

namespace StockControl.API.MediatR.Commands.Organization;

/// <summary>
/// Команда обновления организации
/// </summary>
public record UpdateOrganizationCommand(OrganizationDto? Dto) : IRequest<bool>
{
}