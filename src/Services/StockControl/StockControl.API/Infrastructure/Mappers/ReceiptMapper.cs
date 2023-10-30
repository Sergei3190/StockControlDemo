using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;
using StockControl.API.Models.DTO.Party;
using StockControl.API.Models.DTO.Receipt;
using StockControl.API.Models.DTO.StockAvailability;

namespace StockControl.API.Infrastructure.Mappers;

public static class ReceiptMapper
{
	public static Receipt? CreateEntity(this ReceiptDto dto, Guid userId) => dto is null
		? null
		: new Receipt()
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
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this Receipt entity, ReceiptDto dto, Guid userId)
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
		entity.Number = dto.Number;
		entity.CreateDate = dto.CreateDate;
		entity.CreateTime = dto.CreateTime;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static ReceiptDto? CreateDto(this Receipt entity) => entity is null
		? null
		: new ReceiptDto()
		{
			Id = entity.Id,
			Party = new PartyDto()
			{
				Id = entity.Party.Id,
				Number = entity.Party.Number,
				CreateDate = entity.Party.CreateDate,
				ExtensionNumber = entity.Party.ExtensionNumber,
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
		};

	public static StockAvailabilityDto? CreateStockAvailabilityDto(this ReceiptDto dto, Guid? stockId = null) => dto is null
		? null
		: new StockAvailabilityDto()
		{
			Id = stockId.HasValue ? stockId.Value : Guid.Empty,
			ReceiptId = dto.Id,
			Party = dto.Party,
			Nomenclature = dto.Nomenclature,
			Warehouse = dto.Warehouse,
			Organization = dto.Organization,
			Price = dto.Price,
			Quantity = dto.Quantity,
		};
}
