using GrpcNote;

using Web.StockControl.HttpAggregator.Grpc.ClientServices.Intefaces;
using Web.StockControl.HttpAggregator.Grpc.Models;

namespace Web.StockControl.HttpAggregator.Grpc.ClientServices;

public class NoteGrpcClientService : INoteGrpcClientService
{
	private readonly Note.NoteClient _client;
	private readonly ILogger<NoteGrpcClientService> _logger;

	public NoteGrpcClientService(Note.NoteClient client, ILogger<NoteGrpcClientService> logger)
	{
		_client = client;
		_logger = logger;
	}

	public async Task<bool> UpdateSortAsync(NoteArrayItemModel[] dtoArray)
	{
		var request = MapToNoteArrayRequest(dtoArray);

		_logger.LogTrace("grpc request {@request}", request);
		// несмотря на то, что в файле note.proto у нас не указан префикс Async у метода UpdateSort, мы всё равно можем вызвать его асинхронно.
		var response = await _client.UpdateSortAsync(request);
		_logger.LogTrace("grpc response {@response}", response);

		return response.Flag;
	}

	private NoteArrayRequest? MapToNoteArrayRequest(NoteArrayItemModel[] dtoArray)
	{
		var map = new NoteArrayRequest();

		if (dtoArray is not null)
		{
			foreach (var item in dtoArray)
			{
				map.Items.Add(new NoteArrayItemRequest()
				{
					Id = item.Id,
					Content = item.Content,
					IsFix = item.IsFix,
					Sort = item.Sort,
					ExecutionDate = item.ExecutionDate,
				});
			}
		}

		return map;
	}
}
