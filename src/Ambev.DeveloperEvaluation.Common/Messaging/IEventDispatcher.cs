namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// Defines a contract for retrieving domain events.
/// </summary>
/// <typeparam name="TEvent">The type of the domain event.</typeparam>
public interface IEventDispatcher<TEvent>
{
    /// <summary>
    /// Retrieves all domain events currently managed by the dispatcher.
    /// </summary>
    /// <returns>An enumerable collection of domain events.</returns>
    IEnumerable<TEvent> GetDomainEvents();
}