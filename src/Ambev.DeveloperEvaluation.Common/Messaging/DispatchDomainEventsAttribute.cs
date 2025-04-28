namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// An attribute used to mark a class for dispatching domain events.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class DispatchDomainEventsAttribute : Attribute
{
    /// <summary>
    /// Gets the type of the domain event to be dispatched.
    /// </summary>
    public Type EventType { get; }

    /// <summary>
    /// Gets the list of properties mapped to the domain event.
    /// </summary>
    public string[] MappedProperties { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DispatchDomainEventsAttribute"/> class.
    /// </summary>
    /// <param name="eventType">The type of the domain event to be dispatched. Cannot be null.</param>
    /// <param name="mappedProperties">An optional array of property names to map to the domain event.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="eventType"/> is null.</exception>
    public DispatchDomainEventsAttribute(Type eventType, params string[] mappedProperties)
    {
        EventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
        MappedProperties = mappedProperties ?? [];
    }
}