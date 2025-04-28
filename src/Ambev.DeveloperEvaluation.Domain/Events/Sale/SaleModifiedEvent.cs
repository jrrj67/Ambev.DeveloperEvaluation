using Ambev.DeveloperEvaluation.Common.Messaging;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale;

public class SaleModifiedEvent : IEvent
{
    public Guid Id { get; }
    public string Customer { get; private set; } = string.Empty;
    public string Branch { get; private set; } = string.Empty;
    public bool IsCancelled { get; private set; }

    public DateTime OccurredOn { get; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

    public SaleModifiedEvent(Guid id, string customer, string branch, bool isCancelled)
    {
        Id = id;
        Customer = customer;
        Branch = branch;
        IsCancelled = isCancelled;
    }
}