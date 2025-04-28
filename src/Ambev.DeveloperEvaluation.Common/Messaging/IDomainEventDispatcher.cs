namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// Defines a contract for managing and dispatching domain events.
/// </summary>
/// <typeparam name="TEvent">The type of the domain event. Must be a reference type.</typeparam>
public interface IDomainEventDispatcher<TEvent> where TEvent : class
{
    /// <summary>
    /// Adds a domain event to the dispatcher.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    void AddEvent(TEvent domainEvent);

    /// <summary>
    /// Retrieves all domain events currently managed by the dispatcher.
    /// </summary>
    /// <returns>An enumerable collection of domain events.</returns>
    IEnumerable<TEvent> GetDomainEvents();

    /// <summary>
    /// Clears all domain events from the dispatcher.
    /// </summary>
    void ClearEvents();
}