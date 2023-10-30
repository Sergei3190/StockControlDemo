using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;
using StockControl.API.Models.DTO.Party;
using StockControl.API.Models.DTO.StockAvailability;

namespace StockControl.API.Infrastructure.Mappers;

public static class StockAvailabilityMapper
{
	public static StockAvailability? CreateEntity(this StockAvailabilityDto dto, Guid userId) => dto is null
		? null
		: new StockAvailability()
		{
			Id = dto.Id,
			ReceiptId = dto.ReceiptId,
			MovingId = dto.MovingId,
			WriteOffId = dto.WriteOffId,
			PartyId = dto.Party is null ? throw new ArgumentNullException("Отсутсвует партия товара", nameof(dto.Party)) : dto.Party!.Id,
			NomenclatureId = dto.Nomenclature.Id,
			WarehouseId = dto.Warehouse.Id,
			OrganizationId = dto.Organization.Id,
			Price = dto.Price,
			Quantity = dto.Quantity,
			TotalPrice = dto.TotalPrice,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this StockAvailability entity, StockAvailabilityDto dto, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.NomenclatureId = dto.Nomenclature.Id;
		entity.WarehouseId = dto.Warehouse.Id;
		entity.OrganizationId = dto.Organization.Id;
		entity.Price = dto.Price;
		entity.Quantity = dto.Quantity;
		entity.TotalPrice = dto.TotalPrice;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static StockAvailabilityDto? CreateDto(this StockAvailability entity) => entity is null
		? null
		: new StockAvailabilityDto()
		{
			Id = entity.Id,
			ReceiptId = entity.ReceiptId,
			MovingId = entity.MovingId,
			WriteOffId = entity.WriteOffId,
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
		};
}
