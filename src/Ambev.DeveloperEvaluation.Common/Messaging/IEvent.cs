namespace Ambev.DeveloperEvaluation.Common.Messaging;

/// <summary>
/// Represents a domain event with a timestamp indicating when the event occurred.
/// </summary>
public interface IEvent
{
    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    DateTime OccurredOn { get; }
}