using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// A pipeline behavior that handles the publishing of domain events after a request is processed.
/// </summary>
/// <typeparam name="TRequest">The type of the request being handled. Must be a reference type.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the request handler.</typeparam>
public class DomainEventPublisherBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
{
    private readonly IDomainEventDispatcher<object> _eventDispatcher;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<DomainEventPublisherBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainEventPublisherBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="eventDispatcher">The dispatcher responsible for managing domain events.</param>
    /// <param name="eventPublisher">The publisher responsible for publishing domain events.</param>
    /// <param name="logger">The logger used for logging information and errors.</param>
    public DomainEventPublisherBehavior(
        IDomainEventDispatcher<object> eventDispatcher,
        IEventPublisher eventPublisher,
        ILogger<DomainEventPublisherBehavior<TRequest, TResponse>> logger)
    {
        _eventDispatcher = eventDispatcher;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    /// <summary>
    /// Handles the request, processes domain events, and publishes them if applicable.
    /// </summary>
    /// <param name="request">The incoming request.</param>
    /// <param name="next">The delegate to invoke the next behavior in the pipeline.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The response from the next behavior in the pipeline.</returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        // Check if the request has the DispatchDomainEventsAttribute
        var shouldDispatchEvents = Attribute.IsDefined(request.GetType(), typeof(DispatchDomainEventsAttribute));

        if (shouldDispatchEvents)
        {
            // Retrieve domain events from the request
            var domainEvents = request.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => typeof(IEvent).IsAssignableFrom(prop.PropertyType))
                .Select(prop => prop.GetValue(request))
                .OfType<IEvent>()
                .ToList();

            // Publish each domain event
            foreach (var domainEvent in _eventDispatcher.GetDomainEvents())
            {
                try
                {
                    await _eventPublisher.PublishEventAsync(domainEvent);
                    _logger.LogInformation("Event published: {EventName}", domainEvent.GetType().Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error publishing event: {EventName}", domainEvent.GetType().Name);
                }
            }
        }

        // Clear domain events after publishing
        _eventDispatcher.ClearEvents();
        return response;
    }
}
