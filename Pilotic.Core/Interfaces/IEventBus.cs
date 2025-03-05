namespace Pilotic.Core.Interfaces;

// Event Bus Interface
public interface IEventBus : IInjectableModule
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : IEvent;
}
