using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pilotic.Core.Interfaces;

namespace Pilotic.Core.Services;

public class MemoryEventBus : IEventBus
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<MemoryEventBus> _logger;

    public MemoryEventBus(ILogger<MemoryEventBus> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        using var scope = _serviceScopeFactory.CreateScope();
        
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();;

        if (!handlers.Any())
        {
            _logger.LogWarning("No handlers registered for event {EventType}", typeof(TEvent).Name);
            return;
        }

        foreach (var handler in handlers)
        {
            try {
                await handler.HandleEvent(@event, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling event {EventType} with handler {HandlerType}", typeof(TEvent).Name, handler.GetType().Name);
            }
        }
    }
}
