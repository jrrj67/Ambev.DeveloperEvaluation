namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// A generic class responsible for managing and dispatching domain events.
/// </summary>
/// <typeparam name="TEvent">The type of the domain event. Must be a reference type.</typeparam>
public class DomainEventDispatcher<TEvent> : IDomainEventDispatcher<TEvent> where TEvent : class
{
    private readonly List<TEvent> _domainEvents = [];

    /// <summary>
    /// Adds a domain event to the dispatcher.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    public void AddEvent(TEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Retrieves all domain events currently managed by the dispatcher.
    /// </summary>
    /// <returns>An enumerable collection of domain events.</returns>
    public IEnumerable<TEvent> GetDomainEvents() => _domainEvents;

    /// <summary>
    /// Clears all domain events from the dispatcher.
    /// </summary>
    public void ClearEvents() => _domainEvents.Clear();
}