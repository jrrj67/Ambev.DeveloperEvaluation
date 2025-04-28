namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// Defines a contract for publishing domain events asynchronously.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes a domain event asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the domain event. Must be a reference type.</typeparam>
    /// <param name="domainEvent">The domain event to publish.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PublishEventAsync<T>(T domainEvent) where T : class;
}