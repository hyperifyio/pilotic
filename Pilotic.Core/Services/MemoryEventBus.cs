using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pilotic.Core.Interfaces;

namespace Pilotic.Core.Services;

public class MemoryEventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MemoryEventBus> _logger;

    public MemoryEventBus(IServiceProvider serviceProvider, ILogger<MemoryEventBus> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>().ToList();

        if (!handlers.Any())
        {
            _logger.LogWarning("No handlers registered for event {EventType}", typeof(TEvent).Name);
            return;
        }

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(@event, cancellationToken);
        }
    }
}
