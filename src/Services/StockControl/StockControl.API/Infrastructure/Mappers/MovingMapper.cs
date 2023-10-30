using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;
using StockControl.API.Models.DTO.Moving;
using StockControl.API.Models.DTO.Party;
using StockControl.API.Models.DTO.StockAvailability;

namespace StockControl.API.Infrastructure.Mappers;

public static class MovingMapper
{
	public static Moving? CreateEntity(this MovingDto dto, Guid userId) => dto is null
		? null
		: new Moving()
		{
			Id = dto.Id,
			PartyId = dto.Party is null ? throw new ArgumentNullException("Отсутсвует партия товара", nameof(dto.Party)) : dto.Party!.Id,
			NomenclatureId = dto.Nomenclature.Id,
			WarehouseId = dto.Warehouse.Id,
			OrganizationId = dto.Organization.Id,
			Price = dto.Price,
			Quantity = dto.Quantity,
			TotalPrice = dto.TotalPrice,
			ProductFlowTypeId = dto.ProductFlowType.Id,
			Number = dto.Number,
			CreateDate = dto.CreateDate,
			CreateTime = dto.CreateTime,
			SendingWarehouseId = dto.SendingWarehouse is not null ? dto.SendingWarehouse.Id : null,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this Moving entity, MovingDto dto, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.PartyId = dto.Party.Id;
		entity.NomenclatureId = dto.Nomenclature.Id;
		entity.WarehouseId = dto.Warehouse.Id;
		entity.OrganizationId = dto.Organization.Id;
		entity.Price = dto.Price;
		entity.Quantity = dto.Quantity;
		entity.TotalPrice = dto.TotalPrice;
		entity.Number = dto.Number;
		entity.CreateDate = dto.CreateDate;
		entity.CreateTime = dto.CreateTime;
		entity.SendingWarehouseId = dto.SendingWarehouse is not null ? dto.SendingWarehouse.Id : null;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static MovingDto? CreateDto(this Moving entity) => entity is null
		? null
		: new MovingDto()
		{
			Id = entity.Id,
			Party = new PartyDto()
			{
				Id = entity.Party.Id,
				Number = entity.Party.Number,
				ExtensionNumber = entity.Party.ExtensionNumber,
				CreateDate = entity.Party.CreateDate,
				CreateTime = entity.Party.CreateTime
			},
			Nomenclature = new NamedEntityDto()
			{
				Id = entity.Nomenclature.Id,
				Name = entity.Nomenclature.Name
			},
			Warehouse = new NamedEntityDto()
			{
				Id = entity.Warehouse.Id,
				Name = entity.Warehouse.Name,
			},
			Organization = new NamedEntityDto()
			{
				Id = entity.Organization.Id,
				Name = entity.Organization.Name,
			},
			Price = entity.Price,
			Quantity = entity.Quantity,
			ProductFlowType = new NamedEntityDto()
			{
				Id = entity.ProductFlowType.Id,
				Name = entity.ProductFlowType.Name
			},
			Number = entity.Number,
			CreateDate = entity.CreateDate,
			CreateTime = entity.CreateTime,
			SendingWarehouse = entity.SendingWarehouse is not null ? new NamedEntityDto()
			{
				Id = entity.SendingWarehouse.Id,
				Name = entity.SendingWarehouse.Name,
			} : null,
		};

	public static StockAvailabilityDto? CreateStockAvailabilityDto(this MovingDto dto, Guid? stockId = null) => dto is null
		? null
		: new StockAvailabilityDto()
		{
			Id = stockId.HasValue ? stockId.Value : Guid.Empty,
			MovingId = dto.Id,
			Party = new PartyDto()
			{
				Id = dto.Party.Id,
				Number = dto.Party.Number,
				ExtensionNumber = dto.Party.ExtensionNumber,
				CreateDate = dto.Party.CreateDate,
				CreateTime = dto.Party.CreateTime
			},
			Nomenclature = dto.Nomenclature,
			Warehouse = dto.Warehouse,
			Organization = dto.Organization,
			Price = dto.Price,
			Quantity = dto.Quantity,
		};
}
