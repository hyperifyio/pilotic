namespace Pilotic.Core.Interfaces;

public interface IEventHandler<TEvent> : IInjectableScopedModule where TEvent : IEvent
{
    Task HandleEvent(TEvent @event, CancellationToken cancellationToken = default);
}
