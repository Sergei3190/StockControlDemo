using EventBus.Extensions;

using MediatR;

namespace FileStorage.API.MediatR.Behaviors;

/// <summary>
/// Логирование обработчиков команд и запросов MediatR
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling command {CommandName} ({@Command})", request.GetType().GetGenericTypeName(), request);
        var response = await next();
        _logger.LogInformation("Command {CommandName} handled - response: {@Response}", request.GetType().GetGenericTypeName(), response);

        return response;
    }
}

