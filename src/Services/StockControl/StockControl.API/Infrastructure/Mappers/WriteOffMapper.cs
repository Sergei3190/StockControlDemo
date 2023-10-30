using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;
using StockControl.API.Models.DTO.Party;
using StockControl.API.Models.DTO.StockAvailability;
using StockControl.API.Models.DTO.WriteOff;

namespace StockControl.API.Infrastructure.Mappers;

public static class WriteOffMapper
{
	public static WriteOff? CreateEntity(this WriteOffDto dto, Guid userId) => dto is null
		? null
		: new WriteOff()
		{
			Id = dto.Id,
			PartyId = dto.Party is null ? throw new ArgumentNullException("Отсутсвует партия товара", nameof(dto.Party)) : dto.Party!.Id,
			NomenclatureId = dto.Nomenclature.Id,
			WarehouseId = dto.Warehouse.Id,
			OrganizationId = dto.Organization.Id,
			Price = dto.Price,
			Quantity = dto.Quantity,
			TotalPrice = dto.TotalPrice,
			Reason = dto.Reason,
			ProductFlowTypeId = dto.ProductFlowType.Id,
			Number = dto.Number,
			CreateDate = dto.CreateDate,
			CreateTime = dto.CreateTime,
			SendingWarehouseId = dto.SendingWarehouse is not null ? dto.SendingWarehouse.Id : null,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this WriteOff entity, WriteOffDto dto, Guid userId)
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
		entity.Reason = dto.Reason;
		entity.Number = dto.Number;
		entity.CreateDate = dto.CreateDate;
		entity.CreateTime = dto.CreateTime;
		entity.SendingWarehouseId = dto.SendingWarehouse is not null ? dto.SendingWarehouse.Id : null;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static WriteOffDto? CreateDto(this WriteOff entity) => entity is null
		? null
		: new WriteOffDto()
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
			Reason = entity.Reason,
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

	public static StockAvailabilityDto? CreateStockAvailabilityDto(this WriteOffDto dto, Guid? stockId = null) => dto is null
		? null
		: new StockAvailabilityDto()
		{
			Id = stockId.HasValue ? stockId.Value : Guid.Empty,
			WriteOffId = dto.Id,
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
			Quantity = dto.Quantity
		};
}
