using Service.Common.Entities.App;

using StockControl.API.Domain.Stock;

namespace StockControl.API.Domain;

public static class TestData
{
	private const string TestAdminId = "0E97468A-9710-48FB-B6C4-FCEB9C17D6D5";

	private const string OrganizationMnemo = "Organizations";
	private const string WarehousesMnemo = "Warehouses";
	private const string NomenclatureMnemo = "Nomenclature";

	private const string ReceiptMnemo = "Receipt";

	private const string Organization_1 = "Организация 1";
	private const string Organization_2 = "Организация 2";

	private const string Nomenclature_1 = "Номенклатура 1";
	private const string Nomenclature_2 = "Номенклатура 2";

	private const string Warehouses_1 = "Склад 1";
	private const string Warehouses_2 = "Склад 2";

	private const string Number_1 = "1-849";
	private const string Number_2 = "2-849";
	private const string Number_3 = "3-142";

	public static IEnumerable<UserInfo> Users { get; } = new List<UserInfo>()
	{
		new UserInfo()
		{
			Id = Guid.Parse(TestAdminId),
			Name = "Test",
			Email = "Test Email",
			SourceId = Source.Sources.First().Id
		},
	};

	public static IEnumerable<Organization> Organizations { get; } = new List<Organization>()
	{
		new Organization()
		{
			Id = Guid.NewGuid(),
			Name = Organization_1,
			ClassifierId = Classifier.Classifiers.First(c => c.Mnemo == OrganizationMnemo).Id
		},
		new Organization()
		{
			Id = Guid.NewGuid(),
			Name = Organization_2,
			ClassifierId = Classifier.Classifiers.First(c => c.Mnemo == OrganizationMnemo).Id
		}
	};

	public static IEnumerable<Nomenclature> Nomenclatures { get; } = new List<Nomenclature>()
	{
		new Nomenclature()
		{
			Id = Guid.NewGuid(),
			Name = Nomenclature_1,
			ClassifierId = Classifier.Classifiers.First(c => c.Mnemo == NomenclatureMnemo).Id
		},
		new Nomenclature()
		{
			Id = Guid.NewGuid(),
			Name = Nomenclature_2,
			ClassifierId = Classifier.Classifiers.First(c => c.Mnemo == NomenclatureMnemo).Id
		},
	};

	public static IEnumerable<Warehouse> Warehouses { get; } = new List<Warehouse>()
	{
		new Warehouse()
		{
			Id = Guid.NewGuid(),
			Name = Warehouses_1,
			ClassifierId = Classifier.Classifiers.First(c => c.Mnemo == WarehousesMnemo).Id
		},
		new Warehouse()
		{
			Id = Guid.NewGuid(),
			Name = Warehouses_2,
			ClassifierId = Classifier.Classifiers.First(c => c.Mnemo == WarehousesMnemo).Id
		}
	};

	public static IEnumerable<Party> Parties { get; } = new List<Party>()
	{
		new Party()
		{
			Id = Guid.NewGuid(),
			Number = "1",
			ExtensionNumber = $"#{Guid.NewGuid().ToString().Substring(0,6)}-/{Random.Shared.Next(0, 10_000_000)}{DateTime.Now.Date.Microsecond}",
			CreateDate = DateOnly.FromDateTime(DateTime.Now),
			CreateTime = TimeOnly.FromDateTime(DateTime.Now)
		},
		new Party()
		{
			Id = Guid.NewGuid(),
			Number = "2",
			ExtensionNumber = $"#{Guid.NewGuid().ToString().Substring(0,6)}-/{Random.Shared.Next(0, 10_000_000)}{DateTime.Now.Date.Microsecond}",
			CreateDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
			CreateTime = TimeOnly.FromDateTime(DateTime.Now)
		},
		new Party()
		{
			Id = Guid.NewGuid(),
			Number = "1",
			ExtensionNumber = $"#{Guid.NewGuid().ToString().Substring(0,6)}-/{Random.Shared.Next(0, 10_000_000)}{DateTime.Now.Date.Microsecond}",
			CreateDate = DateOnly.FromDateTime(DateTime.Now),
			CreateTime = TimeOnly.FromDateTime(DateTime.Now)
		},
	};

	public static IEnumerable<Receipt> Receipts { get; } = new List<Receipt>()
	{
		new Receipt()
		{
			Id = Guid.NewGuid(),
			ProductFlowTypeId = ProductFlowType.ProductFlowTypes.First(p => p.Mnemo == ReceiptMnemo).Id,
			Number = Number_1,
			CreateDate = DateOnly.FromDateTime(DateTime.Now),
			CreateTime = TimeOnly.FromDateTime(DateTime.Now),
			PartyId = Parties.First(p => p.Number.Equals("1")).Id,
			OrganizationId =  Organizations.First(c => c.Name == Organization_1).Id,
			WarehouseId =  Warehouses.First(c => c.Name == Warehouses_1).Id,
			NomenclatureId = Nomenclatures.First(c => c.Name == Nomenclature_1).Id,
			Price = 150,
			Quantity = 12,
			TotalPrice = 150 * 12
		},
		new Receipt()
		{
			Id = Guid.NewGuid(),
			ProductFlowTypeId = ProductFlowType.ProductFlowTypes.First(p => p.Mnemo == ReceiptMnemo).Id,
			Number = Number_2,
			CreateDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
			CreateTime = TimeOnly.FromDateTime(DateTime.Now.AddDays(1)),
			PartyId = Parties.First(p => p.Number.Equals("2")).Id,
			OrganizationId =  Organizations.First(c => c.Name == Organization_1).Id,
			WarehouseId =  Warehouses.First(c => c.Name == Warehouses_1).Id,
			NomenclatureId = Nomenclatures.First(c => c.Name == Nomenclature_2).Id,
			Price = 125,
			Quantity = 5,
			TotalPrice = 125 * 5
		},
		new Receipt()
		{
			Id = Guid.NewGuid(),
			ProductFlowTypeId = ProductFlowType.ProductFlowTypes.First(p => p.Mnemo == ReceiptMnemo).Id,
			Number = Number_3,
			CreateDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
			CreateTime = TimeOnly.FromDateTime(DateTime.Now.AddDays(2)),
			PartyId = Parties.Last().Id,
			OrganizationId =  Organizations.First(c => c.Name == Organization_2).Id,
			WarehouseId =  Warehouses.First(c => c.Name == Warehouses_2).Id,
			NomenclatureId = Nomenclatures.First(c => c.Name == Nomenclature_1).Id,
			Price = 120,
			Quantity = 3,
			TotalPrice = 120 * 3
		},
	};

	public static IEnumerable<StockAvailability> StockAvailabilities { get; } = new List<StockAvailability>()
	{
		new StockAvailability()
		{
			Id = Guid.NewGuid(),
			ReceiptId = Receipts.First(p => p.Number == Number_1).Id,
			PartyId = Parties.First(p => p.Number.Equals("1")).Id,
			OrganizationId =  Receipts.First(p => p.Number == Number_1).OrganizationId,
			WarehouseId =  Receipts.First(p => p.Number == Number_1).WarehouseId,
			NomenclatureId = Receipts.First(p => p.Number == Number_1).NomenclatureId,
			Price = Receipts.First(p => p.Number == Number_1).Price,
			Quantity = Receipts.First(p => p.Number == Number_1).Quantity,
			TotalPrice = Receipts.First(p => p.Number == Number_1).TotalPrice
		},
		new StockAvailability()
		{
			Id = Guid.NewGuid(),
			ReceiptId = Receipts.First(p => p.Number == Number_2).Id,
			PartyId = Parties.First(p => p.Number.Equals("2")).Id,
			OrganizationId =  Receipts.First(p => p.Number == Number_2).OrganizationId,
			WarehouseId =  Receipts.First(p => p.Number == Number_2).WarehouseId,
			NomenclatureId = Receipts.First(p => p.Number == Number_2).NomenclatureId,
			Price = Receipts.First(p => p.Number == Number_2).Price,
			Quantity = Receipts.First(p => p.Number == Number_2).Quantity,
			TotalPrice = Receipts.First(p => p.Number == Number_2).TotalPrice
		},
		new StockAvailability()
		{
			Id = Guid.NewGuid(),
			ReceiptId = Receipts.First(p => p.Number == Number_3).Id,
			PartyId = Parties.Last().Id,
			OrganizationId =  Receipts.First(p => p.Number == Number_3).OrganizationId,
			WarehouseId =  Receipts.First(p => p.Number == Number_3).WarehouseId,
			NomenclatureId = Receipts.First(p => p.Number == Number_3).NomenclatureId,
			Price = Receipts.First(p => p.Number == Number_3).Price,
			Quantity = Receipts.First(p => p.Number == Number_3).Quantity,
			TotalPrice = Receipts.First(p => p.Number == Number_3).TotalPrice
		}
	};
}