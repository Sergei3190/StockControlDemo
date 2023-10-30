using MediatR;

namespace StockControl.API.MediatR.Commands.Organization;

/// <summary>
/// Команда удаления организации
/// </summary>
public record DeleteOrganizationCommand(Guid Id) : IRequest<bool>
{
}