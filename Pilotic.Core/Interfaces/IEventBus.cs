namespace Pilotic.Core.Interfaces;

// Event Bus Interface
public interface IEventBus : IInjectableSingletonModule
{
    Task Publish<TEvent>(TEvent @event) 
        where TEvent : IEvent;
}
