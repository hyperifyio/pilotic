using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pilotic.Core.Interfaces;

namespace Pilotic.Core.Services;

/// <summary>
/// An in-memory event bus that uses a channel to queue events and process them in the background.
/// </summary>
public class MemoryEventBus : IEventBus, IDisposable
{
    private readonly ILogger<MemoryEventBus> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly Channel<QueuedEvent> _eventQueue;
    private readonly CancellationTokenSource _cts;
    private readonly Task _backgroundTask;

    private const int MaxConcurrentEvents = 5;

    private class QueuedEvent
    {
        public QueuedEvent(IEvent @event)
        {
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
            CompletionSource = new TaskCompletionSource<IEvent>();
        }

        public IEvent Event { get; }
        public TaskCompletionSource<IEvent> CompletionSource { get; }
    }
    
    public MemoryEventBus(ILogger<MemoryEventBus> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _eventQueue = Channel.CreateUnbounded<QueuedEvent>(new UnboundedChannelOptions { SingleReader = true });
        _cts = new CancellationTokenSource();

        // Start the background event processor
        _backgroundTask = Task.Run(ProcessQueueAsync, _cts.Token);
    }
    
    public void Dispose()
    {
        _cts.Cancel();
        _backgroundTask.Wait();
        _cts.Dispose();
    }
    
    public Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event));

        var queuedEvent = new QueuedEvent(@event);
        _eventQueue.Writer.TryWrite(queuedEvent);
        _logger.LogInformation("Event {EventType} enqueued", typeof(TEvent).Name);
        return queuedEvent.CompletionSource.Task;
    }
    
    private async Task HandleEventAsync(QueuedEvent queuedEvent, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var eventType = queuedEvent.Event.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        var handlers = scope.ServiceProvider.GetServices(handlerType);

        if (!handlers.Any())
        {
            _logger.LogWarning("No handlers registered for event {EventType}", eventType.Name);
            queuedEvent.CompletionSource.SetResult(queuedEvent.Event);
            return;
        }
        
        try
        {
            foreach (dynamic handler in handlers)
            {
                await handler.HandleEvent((dynamic)queuedEvent.Event, cancellationToken);
            }
            queuedEvent.CompletionSource.SetResult(queuedEvent.Event);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling event {EventType}", eventType.Name);
            queuedEvent.CompletionSource.SetException(ex);
        }
    }
    
    private async Task ProcessQueueAsync()
    {
        var semaphore = new SemaphoreSlim(MaxConcurrentEvents);
        await foreach (var eventObj in _eventQueue.Reader.ReadAllAsync(_cts.Token))
        {
            await semaphore.WaitAsync(_cts.Token);
            _ = HandleEventAsync(eventObj, _cts.Token)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted && task.Exception != null)
                    {
                        _logger.LogError(task.Exception, "Unhandled exception during event processing");
                    }
                    semaphore.Release();
                }, _cts.Token);
        }
    }
}
