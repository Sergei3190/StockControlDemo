using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Web.StockControl.HttpAggregator.Infrastructure;

/// <summary>
/// Перехватчик ошибок запросов GRPC
/// </summary>
public class GrpcInterceptor : Interceptor
{
	private readonly ILogger<GrpcInterceptor> _logger;

	public GrpcInterceptor(ILogger<GrpcInterceptor> logger)
	{
		_logger = logger;
	}

	public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
		TRequest request,
		ClientInterceptorContext<TRequest, TResponse> context,
		AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
	{
		var call = continuation(request, context);

		return new AsyncUnaryCall<TResponse>(HandleResponse(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
	}

	private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> task)
	{
		try
		{
			var response = await task;
			return response;
		}
		catch (RpcException e)
		{
			_logger.LogError(e, "Error calling via gRPC: {Status}", e.Status);
			throw;
		}
	}
}
