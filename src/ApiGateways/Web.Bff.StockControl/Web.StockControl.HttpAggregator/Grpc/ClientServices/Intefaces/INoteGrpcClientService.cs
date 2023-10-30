using Web.StockControl.HttpAggregator.Grpc.Models;

namespace Web.StockControl.HttpAggregator.Grpc.ClientServices.Intefaces;

/// <summary>
/// Интерфейс обработки клиентских запросов GRPC к его серверу Заметок
/// </summary>
public interface INoteGrpcClientService
{
	Task<bool> UpdateSortAsync(NoteArrayItemModel[] dtoArray);
}
