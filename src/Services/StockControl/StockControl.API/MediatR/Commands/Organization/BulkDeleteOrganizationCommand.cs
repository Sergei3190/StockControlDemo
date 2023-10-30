using MediatR;

using Service.Common.DTO;

namespace StockControl.API.MediatR.Commands.Organization;

/// <summary>
/// Команда массового удаления организаций
/// </summary>
public record BulkDeleteOrganizationCommand(params Guid[] Ids) : IRequest<BulkDeleteResultDto>
{
}