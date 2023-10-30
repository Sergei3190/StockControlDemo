using MediatR;

using StockControl.API.Models.DTO.Organization;

namespace StockControl.API.MediatR.Commands.Organization;

/// <summary>
/// Команда создания организации
/// </summary>
public record CreateOrganizationCommand(OrganizationDto? Dto) : IRequest<Guid>
{
}